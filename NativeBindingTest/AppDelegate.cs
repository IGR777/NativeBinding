using System.IO;
using CoreData;
using Foundation;
using UIKit;
using System.Runtime.InteropServices;

namespace NativeBindingTest
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        static NSManagedObjectContext _managedObjectContext;
        public static NSManagedObjectContext SharedContext
        {
            get
            {
                if (_managedObjectContext == null)
                {
                    var coordinator = SharedCoordinator;
                    if (coordinator != null)
                    {
                        _managedObjectContext = new NSManagedObjectContext();
                        _managedObjectContext.PersistentStoreCoordinator = coordinator;
                    }
                }
                return _managedObjectContext;
            }
        }

        static NSPersistentStoreCoordinator _persistentStoreCoordinator;
        static NSPersistentStoreCoordinator SharedCoordinator
        {
            get
            {
                if (_persistentStoreCoordinator == null)
                {
                    var storePath = Path.Combine(ApplicationDocumentsDirectory, "History.sqlite");
                    var fileManager = NSFileManager.DefaultManager;

                    if (!fileManager.FileExists(storePath))
                    {
                        var defaultStorePath = NSBundle.MainBundle.PathForResource("History", "sqlite");
                        if (defaultStorePath != null)
                        {
                            var err = new NSError();
                            fileManager.Copy(defaultStorePath, storePath, out err);
                        }
                    }

                    var storeUrl = NSUrl.CreateFileUrl(new[] { storePath });
                    var options = new NSDictionary("MigratePersistentStoresAutomaticallyOption", true, "NSInferMappingModelAutomaticallyOption", true);
                    _persistentStoreCoordinator = new NSPersistentStoreCoordinator(SharedManagedObjectModel);

                    NSError error = null;
                    var result = _persistentStoreCoordinator.AddPersistentStoreWithType(new NSString("NSSQLiteStoreType"), null, storeUrl, options, out error);
                    if (result == null)
                    {
                        System.Console.WriteLine("Unrsolved error");
                        return null;
                    }
                }

                return _persistentStoreCoordinator;
            }
        }

        static string ApplicationDocumentsDirectory
        {
            get
            {
                NSError error = null;
                return NSFileManager.DefaultManager.GetUrl(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, null, false, out error).Path;
            }
        }

        static NSManagedObjectModel SharedManagedObjectModel
        {
            get
            {
                return NSManagedObjectModel.MergedModelFromBundles(null);
            }
        }

    }
}


