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

def getStringDictValue(dictionary, key):
    cfString = CoreFoundation.CFDictionaryGetValue(dictionary, unicode_to_cfstring(key))
    return cfstring_to_unicode(cfString);

def getIntDictValue(dictionary, key):
    nsNumber = CoreFoundation.CFDictionaryGetValue(dictionary, unicode_to_cfstring(key))
    return objc.objc_msgSend(nsNumber, objc.sel_registerName('intValue'))

def getStringProperty(obj, propertyName):
    result = 'None'
    nsString = objc.objc_msgSend(obj, objc.sel_registerName(propertyName))
    if nsString is not None:
        result = ctypes.string_at(objc.objc_msgSend(nsString, objc.sel_registerName('UTF8String')))
    return result

def main():
    NSAutoreleasePool = objc.objc_getClass('NSAutoreleasePool')
    pool = objc.objc_msgSend(NSAutoreleasePool, objc.sel_registerName('alloc'))
    pool = objc.objc_msgSend(pool, objc.sel_registerName('init'))

    NSWorkspace = objc.objc_getClass('NSWorkspace')
    workspace = objc.objc_msgSend(NSWorkspace, objc.sel_registerName('sharedWorkspace'))

    runningApps = objc.objc_msgSend(workspace,objc.sel_registerName('runningApplications'))
    appCount = objc.objc_msgSend(runningApps, objc.sel_registerName('count'))

    allWindowsList = CGWindowListCopyWindowInfo(0, 0)
    allWindowsCount = CoreFoundation.CFArrayGetCount(allWindowsList)

    onScreenWindowsList = CGWindowListCopyWindowInfo((1 << 0),0)
    onScreenWindowsCount = CoreFoundation.CFArrayGetCount(onScreenWindowsList)

    for appIndex in range(appCount):
        app = objc.objc_msgSend(runningApps, objc.sel_registerName('objectAtIndex:'), appIndex)

        bid = getStringProperty(app, 'bundleIdentifier')
        pid = objc.objc_msgSend(app, objc.sel_registerName('processIdentifier'))

        print "{}: {}".format(bid, pid)

        for allWindowIndex in range(allWindowsCount):
            window = CoreFoundation.CFArrayGetValueAtIndex(allWindowsList, allWindowIndex)

            localizedName = getStringProperty(app, 'localizedName')
            windowOwnerName = getStringDictValue(window, 'kCGWindowOwnerName')

            if windowOwnerName == localizedName:
                isHidden = True
                windowNum = 0
                for onScreenWindowsIndex in range(onScreenWindowsCount):
                    onScreenWindow = CoreFoundation.CFArrayGetValueAtIndex(onScreenWindowsList, onScreenWindowsIndex)

                    onScreenWindowNum = getIntDictValue(onScreenWindow, 'kCGWindowNumber')
                    windowNum = getIntDictValue(window, 'kCGWindowNumber')

                    if onScreenWindowNum == windowNum:
                        isHidden =False
                        break
                windowName = getStringDictValue(window, 'kCGWindowName')
                print "     '{}':{}:{}".format(
                    windowName,
                    windowNum,
                    'hidden' if isHidden else 'maximized')

    objc.objc_msgSend(pool, objc.sel_registerName('release'))

if __name__ == '__main__':
    main()