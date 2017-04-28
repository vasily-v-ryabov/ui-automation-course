# -*- coding: utf-8 -*-

import atomac
import time

appleID = "pythonappleid1234@gmail.com"
password = "App12leID"
answer1 = "aaa"
answer2 = "bbb"
answer3 = "ccc"
year = "1994"
firstName = "A"
lastName = "G"
street = "abc"
index = "111111"
city = "NN"
cityCode = "111"
phoneNumber = "1111111"

def getContent(window):
    group1 = window.findFirst(AXRole="AXSplitGroup")
    group2 = group1.groups()[0]
    group3 = group2.groups()[0]
    group4 = group3.findFirst(AXRole="AXScrollArea")
    group5 = group4.findFirst(AXRole="AXWebArea")
    return group5

def getAppleIDWindow(window):
    group1 = getContent(window).findFirst(AXRole="AXGroup", AXSubrole="AXLandmarkContentInfo")
    group2 = group1.groups()[0]
    group3 = group2.groups()[1]
    group4 = group3.groups()[2]
    appStore = group4.findFirst(AXRole="AXLink")
    appStore.Press()
    time.sleep(7)
    list = getContent(window).findAll(AXRole="AXList")[3]
    group5 = list.groups()[0]
    group6 = group5.findFirst(AXRole="AXLink")
    group7 = group6.findFirst(AXRole="AXLink")
    group7.Press()
    time.sleep(5)
    group8 = getContent(window).groups()[2]
    load = group8.buttons()[0]
    load.Press()

def signIn(dialog):
    dialog.findFirst(AXRole='AXTextField', AXIdentifier="_NS:230").setString("AXValue", appleID)
    dialog.findFirst(AXRole='AXTextField', AXIdentifier="_NS:150").setString("AXValue", password)
    time.sleep(0.5)
    window.sendKey('a')
    time.sleep(0.5)
    window.sendKey('<backspace>')
    time.sleep(0.5)
    dialog.findFirst(AXRole='AXButton', AXIdentifier="_NS:88").Press()

def registration(window, dialog):
    create = dialog.findFirstR(AXRole="AXButton", AXIdentifier = "_NS:9")
    create.Press()
    time.sleep(10)
    getContent(window).buttons('Continue')[0].Press()
    time.sleep(5)
    getContent(window).groups()[1].groups()[3].findFirst(AXRole="AXCheckBox").Press()
    getContent(window).buttons('Agree')[0].Press()
    time.sleep(5)
    group = getContent(window).groups()[1]
    group.groups()[1].textFields()[0].setString("AXValue", appleID)
    window.sendKey('<tab>')
    time.sleep(0.5)
    window.sendKey('<tab>')
    time.sleep(0.5)
    group.groups()[2].groups()[1].textFields()[0].setString("AXValue", password)
    window.sendKey('a')
    time.sleep(0.5)
    window.sendKey('<backspace>')
    time.sleep(0.5)
    group.groups()[2].groups()[3].textFields()[0].setString("AXValue", password)
    group.groups()[5].groups()[0].popUpButtons()[0].Press()
    time.sleep(0.5)
    window.sendKey('<cursor_down>')
    time.sleep(0.5)
    window.sendKey('<num_enter>')
    time.sleep(0.5)
    group.groups()[5].groups()[1].textFields()[0].setString("AXValue", answer1)
    group.groups()[6].groups()[0].popUpButtons()[0].Press()
    time.sleep(0.5)
    window.sendKey('<cursor_down>')
    time.sleep(0.5)
    window.sendKey('<num_enter>')
    time.sleep(0.5)
    group.groups()[6].groups()[1].textFields()[0].setString("AXValue", answer2)
    group.groups()[7].groups()[0].popUpButtons()[0].Press()
    time.sleep(0.5)
    window.sendKey('<cursor_down>')
    time.sleep(0.5)
    window.sendKey('<num_enter>')
    time.sleep(0.5)
    group.groups()[7].groups()[1].textFields()[0].setString("AXValue", answer3)
    group.groups()[12].groups()[0].popUpButtons()[0].Press()
    time.sleep(0.5)
    window.sendKey('<cursor_down>')
    time.sleep(0.5)
    window.sendKey('<num_enter>')
    time.sleep(0.5)
    group.groups()[12].groups()[1].popUpButtons()[0].Press()
    time.sleep(0.5)
    window.sendKey('<cursor_down>')
    time.sleep(0.5)
    window.sendKey('<num_enter>')
    time.sleep(0.5)
    group.groups()[12].groups()[2].textFields()[0].setString("AXValue", year)
    getContent(window).buttons('Continue')[0].Press()
    time.sleep(7)
    group = getContent(window).groups()[1]
    group.groups()[6].groups()[0].popUpButtons()[0].Press()
    time.sleep(0.5)
    window.sendKey('<cursor_down>')
    time.sleep(0.5)
    window.sendKey('<num_enter>')
    time.sleep(0.5)
    group.groups()[7].groups()[0].textFields()[0].setString("AXValue", lastName)
    group.groups()[7].groups()[1].textFields()[0].setString("AXValue", firstName)
    group.groups()[8].groups()[0].textFields()[0].setString("AXValue", street)
    group.groups()[9].groups()[0].textFields()[0].setString("AXValue", index)
    group.groups()[9].groups()[1].textFields()[0].setString("AXValue", city)
    group.groups()[10].groups()[0].textFields()[0].setString("AXValue", cityCode)
    group.groups()[10].groups()[1].textFields()[0].setString("AXValue", phoneNumber)
    time.sleep(0.5)
    getContent(window).buttons('Create Apple ID')[0].Press()


atomac.launchAppByBundleId('com.apple.iTunes')
time.sleep(10)
itunes = atomac.getAppRefByBundleId('com.apple.iTunes')
window = itunes.windows()[0]

signOut = itunes.menuItem('Account', 'Sign Out')
if(signOut):
    signOut.Press()
    time.sleep(5)

itunes.menuItem('Account', 1).Press()
time.sleep(7)
signIn(itunes.windows()[0])
time.sleep(7)
if itunes.windows()[0].AXIdentifier == "_NS:15":
    itunes.windows()[0].findFirst(AXRole = "AXButton", AXSubrole = "AXCloseButton").Press()
    time.sleep(2)
    getAppleIDWindow(window)
    time.sleep(2)
    dialog = itunes.windows()[0]
    registration(window, dialog)
    print "Registration completed"
else:
    print "Authorization completed"