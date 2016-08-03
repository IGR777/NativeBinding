using ObjCRuntime;

[assembly: LinkWith ("GnSDKObjC.framework", 
                     LinkTarget.ArmV7 | LinkTarget.Simulator | LinkTarget.Arm64 | LinkTarget.Simulator64 | LinkTarget.i386, 
                     SmartLink = true,
                     Frameworks= "CoreData Foundation AVFoundation AudioToolbox CoreMedia Security",
					 //Frameworks = "CoreData Foundation",
					 LinkerFlags = "-lxml2 -ObjC -lz -lstdc++",
                     ForceLoad = true)]