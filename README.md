# Yealink Dialer
This utility provides a interface for handling CALLTO links and forwarding them to Yealink SIP Phones. <br>
Manual dialing of a number is also possible.(This is __NOT__ a SoftPhone)<br>

![image](https://github.com/user-attachments/assets/9cf0563a-d393-4c38-a1b5-4ffa13a2f83f)
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
To registers the application for the first time, run `register.bat`. <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/d146a6ff-914f-4717-aea5-1f7a56bba78f)
<br>
This will setup all nesseary registry entries to handle the callto links <br>
If you ever move the folder, run this again to update the paths. <br>
<br>
After running register.bat, Yealink Dialer should open it self. <br>
When launching for the first time, the settings menu should automatically open. <br>
If not then you can find them at the bottom right of the call window.<br>
![image](https://github.com/user-attachments/assets/6ff2caba-6642-4ab6-8074-6dde44db2ff9)
<br>
<br>
Change these settings to reflect the correct settings of your Yealink device. <br>
<br>
`Phone IP` Specify the IP of the YeaLink Phone that the calls should be dialed with. <br>
`Sip Account`  Specify the SIP account the call should be made with. The format is USERNAME@SIPSERVERIP <br>
`Sip Username`   The username used to login to your Yealink phone's web interface. <br>
`Sip Password`   The password used to login to your Yealink phone's web interface. <br>
<br>
`Autoclose` When launched trough a callto link, the dial window will auto close after the set time. <br>
`Autodial` Checking this box will automatically initiate the call after a set delay, when launched by a callto link. <br>

<br>
You can now hit save. <br>
<br>
For this utility to work, you need to configure the Action URI Allow IP List <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/18bb20c9-dc72-439f-b1b5-fddcc090b5cb)
<br>
You can either specify your PC IP and limit it to just you, or use a wildcards in the IP field. <br>
<br>
After this step is done, you can hit the test button. <br>
It will ask you to confirm the test and will try to make a call to 012345767890 once you do. <br>
The first time you do this, or when ever you change these settings, your Yealink Phone will ask you for permission. <br>
Look at your phones screen and confirm that you want to allow remote access. <br>
<br>
If you do not see any message on your Yealink Device, and it does not dial anything, check your settings again. <br>
<br>
If everything is setup correctly, you should now be able to use callto: tel: sip: and phone: links or you can initiate a call by using the manual dial. <br>
<br>
