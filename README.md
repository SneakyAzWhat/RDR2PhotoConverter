# RDR2 Photo Converter

<p align="center">
  <img width="640" height="450" src="https://github.com/SneakyAzWhat/RDR2PhotoConverter/blob/master/preview.png">
</p>

&nbsp;  
## Application Description
Convert photos that were captured with the game camera in [PC] Red Dead Redemption 2. These files (singleplayer and multiplayer) are stored on your PC with the name 'PRDRxxxxxxx' in \Documents\Rockstar Games\Red Dead Redemption 2\Profiles\'profileName'. My application takes each Rockstar PRDR file at the specified path and converts it into a .jpg format that you can view/interact with like any other image on your PC. During this conversion process I do read the 'metadata' of the date and time when the picture was taken, I have labeled the files with that information to make organization easier. Rockstar stores the files at a max size of 1MB, therefore images may not be in the highest of quality compared to using an alternative capture method in the first place (such as ShadowPlay, Steam Capture etc).

## Why?
This automates the process of converting, backing up and deleting X amount of files instead of manually going through each one. Personally I hated that there is a limit to how many photos you can have in your camera roll and didn't want to upload every single one to social club. This allows me to quickly create space for new photos while also giving me access to my pictures with standard image viewing programs since they are now just .jpg!

**NOTE**:&nbsp;  
It is likely that you'll get a windows security warning and/or an antivirus prompt if for some reason you have one installed (talking about norton, mcaffee etc). This is expected behavior as I don't have a certificate nor have I looked into the process of how to disable those warnings for windows. As a result this application source code is available on my github to show what the program is doing, I have included little notes in the MainWindow.xaml.cs file to describe what is happening for people who may not be familiar with programming. If you have ANY further questions don't hesitate to ask me. If you're an experienced programmer, then I apologize for the verbose code/comments and overall mess! :P

## Instructions
>[Youtube video walkthrough if you prefer visual/audio to text](http://www.github.com/) **---- add the youtube video walkthrough link**


1. [Download the ZIP from here](https://github.com/SneakyAzWhat/RDR2PhotoConverter/releases/download/v1.0/RDR2PhotoConverter.zip)
2. Unzip it in your Documents folder (or wherever you'd like to store it)
3. Run the 'RDR2 Photo Converter.exe' (Windows/antivirus may pop a warning here as mentioned above)
4. -- You may be [asked](https://github.com/SneakyAzWhat/RDR2PhotoConverter/blob/main/dotnet5popup.png) to download '.net 5', the app depends on this windows functionality so click yes and download the x64 version, some of you may have it installed already. --
5. By default the program will retrieve the default path for these RDR2 photo files. If you've been manually moving the PRDR files to another path, you can select 'Custom Path' and paste in that path to convert them.
6. Click the **Set Directory** button so the application knows which path you're using
7. Decide if you want to backup/delete your prdr files during the conversion process and check the appropriate boxes (by default I have it backup your files. Deleting the files is useful to create space for more pictures)
8. Click the **Convert** button, your files will be converted in addition to backed up/deleted based on your checkboxes


>pictures are stored in \Pictures\RDR2 Photos  
>backups of your prdr files are stored in \Pictures\RDR2 Photos\prdr backups  

&nbsp;   
I'd like to give credit to the following [person/thread](https://www.reddit.com/r/PCRedDead/comments/dvejz5/rdr2_photomode_extractor/) for giving me the idea. Creating a desktop (WPF) application would be a fun little learning process, plus it makes things much easier for the average user to access and consume.

Finally, I want to say thanks to the handful of people who helped test this out to make sure it functions properly (and without any errors riiight?). I was originally going to take time and turn this into an image viewer as well, but I am content with this little app and will look forward to working on some new projects. This was mainly for personal use but I figured sharing it could prove beneficial for others who rely on the in-game camera. If there is a serious bug or an overwhelming request I will set aside time to address that and update the app. Otherwise thanks for your time and enjoy :)

P.S - Create an issue on github, comment on the reddit thread or reach out to me on [twitter](https://twitter.com/sneakyazwhat) or [discord](https://discord.gg/YjP2Js7W8K) if something breaks or you need to get in touch.
