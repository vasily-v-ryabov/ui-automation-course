from __future__ import print_function

from objc_bindings import CoreFoundation
from objc_bindings import Objc
from objc_bindings import Quartz
from objc_constants import kCGNullWindowID
from objc_constants import kCGWindowListOptionAll
from objc_constants import kCGWindowListOptionOnScreenOnly


def main():
    objc = Objc()
    quartz = Quartz()
    cf = CoreFoundation()

    ns_auto_release_pool = objc.get_class('NSAutoreleasePool')
    pool = objc.call_selector(ns_auto_release_pool, 'alloc')
    pool = objc.call_selector(pool, 'init')

    ns_workspace = objc.get_class('NSWorkspace')
    workspace = objc.call_selector(ns_workspace, 'sharedWorkspace')

    running_apps = objc.call_selector(workspace, 'runningApplications')
    app_count = objc.call_selector(running_apps, 'count')

    all_windows_list = quartz.cg_window_list_copy_window_info(kCGWindowListOptionAll, kCGNullWindowID)
    all_windows_count = cf.cf_array_get_count(all_windows_list)

    on_screen_windows_list = quartz.cg_window_list_copy_window_info(kCGWindowListOptionOnScreenOnly, kCGNullWindowID)
    on_screen_windows_count = cf.cf_array_get_count(on_screen_windows_list)

    for app_index in range(app_count):
        app = objc.call_selector_with_arg(running_apps, 'objectAtIndex', app_index)

        bid = objc.get_string_property(app, 'bundleIdentifier')
        pid = objc.call_selector(app, 'processIdentifier')
        print("{}: {}".format(bid, pid))

        for all_windows_index in range(all_windows_count):
            window = cf.cf_array_get_value_at_index(all_windows_list, all_windows_index)

            localized_name = objc.get_string_property(app, 'localizedName')
            window_owner_name = cf.get_string_dict_value(window, 'kCGWindowOwnerName')

            if window_owner_name == localized_name:
                is_hidden = True
                window_num = 0
                for on_screen_windows_index in range(on_screen_windows_count):
                    on_screen_window = cf.cf_array_get_value_at_index(on_screen_windows_list,
                                                                      on_screen_windows_index)

                    on_screen_window_num = get_int_dict_value(cf, objc, on_screen_window, 'kCGWindowNumber')
                    window_num = get_int_dict_value(cf, objc, window, 'kCGWindowNumber')

                    if on_screen_window_num == window_num:
                        is_hidden = False
                        break

                window_name = cf.get_string_dict_value(window, 'kCGWindowName')
                print("     '{}':{}:{}".format(
                    window_name,
                    window_num,
                    'hidden' if is_hidden else 'maximized'))

    objc.call_selector(pool, 'release')


def get_int_dict_value(cf, objc, cf_dictionary_ref, key):
    on_screen_window_num_ns = cf.cf_dictionary_get_value(cf_dictionary_ref,
                                                         cf.unicode_to_cf_string(key))
    on_screen_window_num = objc.call_selector(on_screen_window_num_ns, 'intValue')
    return on_screen_window_num


if __name__ == '__main__':
    main()
