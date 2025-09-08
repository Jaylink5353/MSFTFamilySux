# How to get Administrator Priveleges
Currently, this guide is a Work in Progress, please report any errors!
You are going to want to either save these steps on another device or write it down.
You will need to know how to use the command prompt for this! Note the folder where the three Batch files are, as well!

## 1. Turning On Screen Keyboard into Command Prompt
This will get you an admin command prompt from which you can make your user an admin.

### 1.A Getting to Windows RE
Click Restart Into WinRE To restart into the Windows Recovery Enviornment. You will see a screen like this:
//INSERT IMAGE HERE

We will be using this to get an admin command prompt. Click Troubleshoot, then Advanced Options, then Command Prompt. It may restart your computer again, this is expected. You may encounter it needing a admin's password, if so, refer to this guide to make a dummy account: {NEXT GUIDE HERE}

Once you get to the command prompt, type the following:
```
C:
```
If you get an error, follow along. Else, skip to part 1.B.
Type the following:
```
diskpart
```
Once you get into diskpart, type
```
list disk
```
Find which disk contains your Windows Install, then
```
select disk x
```
(Where x is your disk). Then,
```
list partition
```
Find which one contains the C:\Windows folder, then
```
select partition x
```
then, 
``` 
assign letter=C
```
Then type `exit`, then `C:`, and you may proceed to step 1.B.

### 1.B Moving Files
Earlier, you noted where your .bat files are. Now its time to use them!
```
cd C:\Path\to\batch\scripts
```
Then, type `dir` and make sure "giveCMD.bat" is in the list. Now, lets start the process:
```
.\giveCMD.bat
```
Once the script completes, close out of CMD, and click "Continue". Now, when you get to the login screen, click the accessibility icon (image below), then On Screen Keyboard. If it worked correctly, you should have a command prompt! Now, move on to step 2.

## 2. Converting the User to an Admin
Occasionally during this process, you may get errors saying `The system cannot find message .....`, this is fine just ignore it.

Earlier, you noted where the folder with the script is, so we are going to go back to it.
```
cd C:\Path\to\Script
```
Type dir to veriy giveADMIN.bat is in the list, then type
```
net users
```
From the list, find your username, then copy it. Then, type
```
.\giveADMIN.bat "<PASTE USERNAME HERE>"
```
Be sure there are quotes around the username. Once the script finishes, restart your computer. Proceed to step 3

## 3. Cleaning Up