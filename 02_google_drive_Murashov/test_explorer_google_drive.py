# encoding: utf-8
from os import path
from pywinauto import Desktop, Application

chrome_dir = r'"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe"'
tab_log_in = u'Meet Google Drive - One place for all your files (Incognito)'
tab_drive = 'My Drive - Google Drive (Incognito)'

# # start google chrome
chrome = Application(backend='uia')
chrome.start(chrome_dir + ' --force-renderer-accessibility --incognito --start-maximized '
             'https://accounts.google.com/ServiceLogin?service=wise&passive=1209600&continue=https:'
             '//drive.google.com/?urp%3Dhttps://www.google.ru/_/chrome/newtab?espv%253D2%2526ie%253DUT%23&followup='
             'https://drive.google.com/?urp%3Dhttps://www.google.ru/_/chrome/newtab?espv%253D2%2526ie%253DUT'
             '&ltmpl=drive&emr=1#identifier')

# wait while a page is loading
chrome[tab_log_in].child_window(title_re='Reload.*', control_type='Button').wait('visible', timeout=10)
ch_window = chrome[tab_drive].child_window(title="Google Chrome", control_type="Custom")

# log in
chrome.window().type_keys('TestPywinauto{ENTER}')  # username
chrome[tab_log_in].child_window(title="Google Chrome", control_type="Custom").\
    child_window(title="Back", control_type="Image").wait(wait_for='visible', timeout=10)

chrome.window().type_keys('testpywinauto123{ENTER}')  # password
ch_window.child_window(title="Getting started PDF", control_type="ListItem").wait(wait_for='visible')

# start explorer in the current path
dir_path = path.join(path.dirname(path.realpath(__file__)), 'UIA_Drive')
explorer = Application().start('explorer.exe ' + dir_path)
Desktop(backend='uia').window(title='UIA_Drive', active_only=True).wait('visible', timeout=10)
dlg = Application(backend='uia').connect(path='explorer.exe', title='UIA_Drive')

# find file
file = dlg.UIA_Drive.ItemsView.wrapper_object().get_item('test.zip')

# drag n drop
destination = chrome.top_window().wrapper_object()
file.press_mouse_input(coords=file.rectangle().mid_point())
file.move_mouse_input(coords=destination.rectangle().mid_point())
chrome.top_window().set_focus()
file.release_mouse_input(coords=destination.rectangle().mid_point())

# wait upload file
ch_window.child_window(title="test.zip Compressed Archive", control_type="ListItem").wait('visible')

print('DONE')
