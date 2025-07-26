# Yealink Dialer
This utility provides a interface for handling CALLTO links and forwarding them to Yealink SIP Phones. <br>
Manual dialing of a number is also possible. (This is __NOT__ a SoftPhone)<br>

![image](https://github.com/user-attachments/assets/598375f6-03c1-43e1-89b0-32dd45f59e45)
<br>
<br>
# How to use
Download the latest version from Releases. <br>
https://github.com/DJFPaul/yealink_dialer/releases <br>
<br>
Unzip folder to a directory of choice. <br>
It is recommended not to use a directory that requires admin write permissions. <br>
If you choose such, you will need to launch the utility as admin when ever you change settings. <br>
<br>
To register the application for the first time, run `register.bat`. <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/d146a6ff-914f-4717-aea5-1f7a56bba78f)
<br>
This will setup all necessary registry entries to handle the callto links <br>
If you ever move the folder, run this again to update the paths. <br>
<br>
After running `register.bat`, Yealink Dialer should open it self. <br>
When launching for the first time, the Setup Wizard should automatically open. <br>
This will guide you trough the setup process nesseary for using this tool. <br>
Make sure to hit save after you are done! <br>
<br>
If it does not open, you should be greeted with the general settings page where you can launch it manually at the top right. <br>
<br>
Should the Setup Wizard not work for you or you want to use the normal settings, you can follow the next instructions from here as well. <br>
If the settings did not automatically open, you can find them at the bottom right of the call window.<br>
<br>
<img width="229" height="412" alt="image" src="https://github.com/user-attachments/assets/cfd64a68-f26b-46e2-afed-ce5307605229" />
<br>
<br>
Change these settings to reflect the correct settings of your Yealink device. <br>
<br>
`Phone IP` Specify the IP of the YeaLink Phone that the calls should be dialed with. <br>
`Sip Account`  You may specify the SIP account the call should be made with. The format is `USERNAME@SIPSERVERIP` <br>
`Sip Username`   The username used to login to your Yealink phone's web interface. <br>
`Sip Password`   The password used to login to your Yealink phone's web interface. <br>
<br>
`Autoclose` When launched trough a callto link, the dial window will auto close after the set time. <br>
`Autodial` Checking this box will automatically initiate the call after a set delay, when launched by a callto link. <br>
<br>
For this utility to work, you need to configure the Action URI Allow IP List <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/18bb20c9-dc72-439f-b1b5-fddcc090b5cb)
<br>
You can either specify your PC IP and limit it to just you, or use a wildcard in the IP field. <br>
<br>
After this step is done, you can hit the test button. <br>
It will ask you to confirm the test and will try to make a call to 012345767890 once you do. <br>
The first time you do this, or when ever you change these settings, your Yealink Phone will ask you for permission. <br>
Look at your phones screen and confirm that you want to allow remote access. <br>
<br>
If you do not see any message on your Yealink Device, and it does not dial anything, check your settings again. <br>
<br>
If everything is setup correctly, you should now be able to use callto: tel: sip: and phone: links or you can initiate a call by using the manual dial. <br>
Hit save and try it out.
<br>
