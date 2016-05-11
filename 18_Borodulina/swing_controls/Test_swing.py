from jpype import *
from time import sleep

sleepInterval = 0.4

startJVM(getDefaultJVMPath(),
         "-ea -cp .")

testPkg = JPackage('org').wg3i.test
Example = testPkg.Example
it = Example()
it.setVisible(True)
sleep(1)

it.clickOnButton()
sleep(1)

it.clickOnCheckBox()
sleep(1)

it.clickOnButton()
sleep(1)

it.clickOnCheckBox()
sleep(1)

it.inputText("T")
sleep(sleepInterval)
it.inputText("Ty")
sleep(sleepInterval)
it.inputText("Typ")
sleep(sleepInterval)
it.inputText("Typi")
sleep(sleepInterval)
it.inputText("Typing")
sleep(sleepInterval)
it.inputText("Typing.")
sleep(sleepInterval)
it.inputText("Typing..")
sleep(sleepInterval)
it.inputText("Typing...")

sleep(10)

shutdownJVM()