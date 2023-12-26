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
It is recommended not to use a directory that required admin write permissions. <br>
If you choose such, you will need to launch the utility as admin when ever you change settings. <br>
<br>
To registers the application for the first time, run `register.bat`. <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/d146a6ff-914f-4717-aea5-1f7a56bba78f)
<br>
This will setup all nesseary registry entries to handle the callto links <br>
If you ever move the folder, run this again to update the paths. <br>
<br>
After running register.bat, Yealink Dialer should open it self. <br>
When launching the application without a callto link, it will present the cofing interface. <br>

![image](https://github.com/DJFPaul/yealink_dialer/assets/35642602/0079859b-43bb-420e-8a45-8b1c9a188362)
<br>
Change these settings to reflect the correct settings of your Yealink device. <br>
<br>
`PHONEIP`  Specify the IP of the YeaLink Phone that the calls should be dialed with. <br>
`SIPACCOUNT`  Specify the SIP account the call should be made with. The format is USER@IP <br>
`USERNAME`   The username used to login to your Yealink phone's web interface. <br>
`PASSWORD`   The password used to login to your Yealink phone's web interface. <br>
<br>
