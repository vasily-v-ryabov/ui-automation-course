from __future__ import print_function

import ctypes
import ctypes.util

from CoreFoundation import *

objc = ctypes.cdll.LoadLibrary(ctypes.util.find_library('objc'))
quartz = ctypes.cdll.LoadLibrary(ctypes.util.find_library('Quartz'))

objc.objc_getClass.restype = ctypes.c_void_p
objc.sel_registerName.restype = ctypes.c_void_p
objc.objc_msgSend.restype = ctypes.c_void_p
objc.objc_msgSend.argtypes = [ctypes.c_void_p, ctypes.c_void_p]

CGWindowListCopyWindowInfo = quartz.CGWindowListCopyWindowInfo
CGWindowListCopyWindowInfo.restype = CFArrayRef
CGWindowListCopyWindowInfo.argtypes = [ctypes.c_uint32, ctypes.c_uint32]

def get_string_dict_value(dictionary, key):
    cf_string = CoreFoundation.CFDictionaryGetValue(dictionary, unicode_to_cfstring(key))
    return cfstring_to_unicode(cf_string);

def get_int_dict_value(dictionary, key):
    ns_number = CoreFoundation.CFDictionaryGetValue(dictionary, unicode_to_cfstring(key))
    return objc.objc_msgSend(ns_number, objc.sel_registerName('intValue'))

def get_string_property(obj, property_name):
    result = 'None'
    ns_string = objc.objc_msgSend(obj, objc.sel_registerName(property_name))
    if ns_string is not None:
        result = ctypes.string_at(objc.objc_msgSend(ns_string, objc.sel_registerName('UTF8String')))
    return result

def main():
    NSAutoreleasePool = objc.objc_getClass('NSAutoreleasePool')
    pool = objc.objc_msgSend(NSAutoreleasePool, objc.sel_registerName('alloc'))
    pool = objc.objc_msgSend(pool, objc.sel_registerName('init'))

    NSWorkspace = objc.objc_getClass('NSWorkspace')
    workspace = objc.objc_msgSend(NSWorkspace, objc.sel_registerName('sharedWorkspace'))

    running_apps = objc.objc_msgSend(workspace,objc.sel_registerName('runningApplications'))
    app_count = objc.objc_msgSend(running_apps, objc.sel_registerName('count'))

    all_windows_list = CGWindowListCopyWindowInfo(0, 0)
    all_windows_count = CoreFoundation.CFArrayGetCount(all_windows_list)

    on_screen_windows_list = CGWindowListCopyWindowInfo((1 << 0),0)
    on_screen_windows_count = CoreFoundation.CFArrayGetCount(on_screen_windows_list)

    for app_index in range(app_count):
        app = objc.objc_msgSend(running_apps, objc.sel_registerName('objectAtIndex:'), app_index)

        bid = get_string_property(app, 'bundleIdentifier')
        pid = objc.objc_msgSend(app, objc.sel_registerName('processIdentifier'))

        print("{}: {}".format(bid, pid))

        for all_windows_index in range(all_windows_count):
            window = CoreFoundation.CFArrayGetValueAtIndex(all_windows_list, all_windows_index)

            localized_name = get_string_property(app, 'localizedName')
            window_owner_name = get_string_dict_value(window, 'kCGWindowOwnerName')

            if window_owner_name == localized_name:
                is_hidden = True
                window_num = 0
                for on_screen_windows_index in range(on_screen_windows_count):
                    on_screen_window = CoreFoundation.CFArrayGetValueAtIndex(on_screen_windows_list, on_screen_windows_index)

                    on_screen_window_num = get_int_dict_value(on_screen_window, 'kCGWindowNumber')
                    window_num = get_int_dict_value(window, 'kCGWindowNumber')

                    if on_screen_window_num == window_num:
                        is_hidden =False
                        break
                window_name = get_string_dict_value(window, 'kCGWindowName')
                print("     '{}':{}:{}".format(
                    window_name,
                    window_num,
                    'hidden' if is_hidden else 'maximized'))

    objc.objc_msgSend(pool, objc.sel_registerName('release'))

if __name__ == '__main__':
    main()