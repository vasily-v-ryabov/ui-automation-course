import ctypes.util
from ctypes import c_char_p

from ObjcConstants import CFStringRef, CFStringEncoding, CFIndex, CFAllocatorRef, CFTypeRef, \
    CFDictionaryRef, \
    CFErrorRef, kCFStringEncodingUTF8, CFArrayRef


class CoreFoundation:
    def __init__(self):
        self.__core_foundation = ctypes.cdll.LoadLibrary(ctypes.util.find_library('CoreFoundation'))
        self.__init_core_foundation()

    def __init_core_foundation(self):
        self.__core_foundation.CFStringGetCStringPtr.argtypes = [CFStringRef, CFStringEncoding]
        self.__core_foundation.CFStringGetCStringPtr.restype = c_char_p

        self.__core_foundation.CFStringGetCString.argtypes = [CFStringRef, c_char_p, CFIndex, CFStringEncoding]
        self.__core_foundation.CFStringGetCString.restype = ctypes.c_bool

        self.__core_foundation.CFStringCreateWithCString.argtypes = [CFAllocatorRef, c_char_p, CFStringEncoding]
        self.__core_foundation.CFStringCreateWithCString.restype = CFStringRef

        self.__core_foundation.CFArrayGetCount.argtypes = [CFArrayRef]
        self.__core_foundation.CFArrayGetCount.restype = CFIndex

        self.__core_foundation.CFArrayGetValueAtIndex.argtypes = [CFArrayRef, CFIndex]
        self.__core_foundation.CFArrayGetValueAtIndex.restype = CFTypeRef

        self.__core_foundation.CFDictionaryGetValue.restype = CFStringRef
        self.__core_foundation.CFDictionaryGetValue.argtypes = [CFDictionaryRef, CFStringRef]

        setattr(self.__core_foundation, 'kCFAllocatorDefault',
                CFAllocatorRef.in_dll(self.__core_foundation, 'kCFAllocatorDefault'))
        setattr(self.__core_foundation, 'CFIndex', CFIndex)
        setattr(self.__core_foundation, 'CFStringRef', CFStringRef)
        setattr(self.__core_foundation, 'CFTypeRef', CFTypeRef)
        setattr(self.__core_foundation, 'CFAllocatorRef', CFAllocatorRef)
        setattr(self.__core_foundation, 'CFArrayRef', CFArrayRef)
        setattr(self.__core_foundation, 'CFDictionaryRef', CFDictionaryRef)
        setattr(self.__core_foundation, 'CFErrorRef', CFErrorRef)
        pass

    def cf_array_get_count(self, cf_array_ref):
        return self.__core_foundation.CFArrayGetCount(cf_array_ref)

    def cf_dictionary_get_value(self, cf_dictionary_ref, key):
        return self.__core_foundation.CFDictionaryGetValue(cf_dictionary_ref, key)

    def cf_array_get_value_at_index(self, cf_array_ref, cf_index):
        return self.__core_foundation.CFArrayGetValueAtIndex(cf_array_ref, cf_index)

    def get_string_dict_value(self, cf_dictionary_ref, key):
        cf_string = self.cf_dictionary_get_value(cf_dictionary_ref, self.unicode_to_cf_string(key))
        return self.cf_string_to_unicode(cf_string)

    def cf_string_to_unicode(self, value):
        string = self.__core_foundation.CFStringGetCStringPtr(
            ctypes.cast(value, CFStringRef),
            kCFStringEncodingUTF8
        )

        if string is None:
            string = 'None'
            # buf = ctypes.create_string_buffer(1024)
            # result = CoreFoundation.CFStringGetCString(
            #     ctypes.cast(value, CFStringRef),
            #     buf,
            #     1024,
            #     kCFStringEncodingUTF8
            # )
            # if not result:
            #     raise OSError('Error copying C string from CFStringRef')
            # string = buf.value
        if string is not None:
            string = string.decode('utf-8')
        return string

    def unicode_to_cf_string(self, value):
        return self.__core_foundation.CFStringCreateWithCString(
            self.__core_foundation.kCFAllocatorDefault,
            value.encode('utf-8'),
            kCFStringEncodingUTF8
        )


class Quartz:
    def __init__(self):
        self.__quartz = ctypes.cdll.LoadLibrary(ctypes.util.find_library('Quartz'))
        self.__init_quartz()

    def __init_quartz(self):
        self.__quartz.CGWindowListCopyWindowInfo.restype = CFArrayRef
        self.__quartz.CGWindowListCopyWindowInfo.argtypes = [ctypes.c_uint32, ctypes.c_uint32]
        pass

    def cg_window_list_copy_window_info(self, option, relative_to_window):
        return self.__quartz.CGWindowListCopyWindowInfo(option, relative_to_window)


class Objc:
    def __init__(self):
        self.__objc = ctypes.cdll.LoadLibrary(ctypes.util.find_library('objc'))
        self.__init_objc()

    def __init_objc(self):
        self.__objc.objc_getClass.restype = ctypes.c_void_p
        self.__objc.sel_registerName.restype = ctypes.c_void_p
        self.__objc.objc_msgSend.restype = ctypes.c_void_p
        self.__objc.objc_msgSend.argtypes = [ctypes.c_void_p, ctypes.c_void_p]
        pass

    def get_class(self, name):
        return self.__objc.objc_getClass(name)

    def call_selector(self, obj, selector_name):
        return self.__objc.objc_msgSend(obj, self.__objc.sel_registerName(selector_name))

    def call_selector_with_arg(self, obj, selector_name, arg):
        return self.__objc.objc_msgSend(obj, self.__objc.sel_registerName(selector_name + ':'), arg)

    def get_string_property(self, obj, property_name):
        result = 'None'
        ns_string = self.__objc.objc_msgSend(obj, self.__objc.sel_registerName(property_name))
        if ns_string is not None:
            result = ctypes.string_at(self.__objc.objc_msgSend(ns_string, self.__objc.sel_registerName('UTF8String')))
        return result
