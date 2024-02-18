# Yealink Dialer
This utility provides a interface for handling CALLTO links and forwarding them to Yealink SIP Phones. <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/868973ba-d89e-45eb-b88a-e67ee1ad97fd)
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
When launching the application without a callto link, it will present the config interface. <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/0079859b-43bb-420e-8a45-8b1c9a188362)
<br>
Change these settings to reflect the correct settings of your Yealink device. <br>
<br>
`PHONEIP`  Specify the IP of the YeaLink Phone that the calls should be dialed with. <br>
`SIPACCOUNT`  Specify the SIP account the call should be made with. The format is USERNAME@SIPSERVERIP <br>
`USERNAME`   The username used to login to your Yealink phone's web interface. <br>
`PASSWORD`   The password used to login to your Yealink phone's web interface. <br>
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
If everything is setup correctly, you should now be able to click on callto: and tel: links and it should open up Yealink Dialer. <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/868973ba-d89e-45eb-b88a-e67ee1ad97fd)
<br>

# Optional Options
In the data folder, you can find a `config.ini` with some additional options.<br>
<br>
`autoclose=true` The dial window will auto close after a set time. <br>
`closedelay=8` Specifies the delay after which to auto close. <br>
`autodial=false` Setting this to true will automatically try to call any number you click. <br>
`dialdelay` Specifies the time to way until autodial will forward the call request. <br>
