﻿using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.Ifc2x3.SharedBldgElements;
using Xbim.IO;

namespace Xbim.MemoryModel.Tests
{
    [TestClass]
    public class ParsingTests
    {

        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void IfcStoreOpenAndCloseMemoryModelTest()
        {
            using (var store = IfcStore.Open("4walls1floorSite.ifc"))
            {
                var count = store.Instances.Count;
                Assert.IsTrue(count > 0, "Should have more than zero instances");
                store.Close();
            }
        }

        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void IfcStoreOpenAndCloseEsentModelTest()
        {
            using (var store = IfcStore.Open("4walls1floorSite.ifc",null,0))
            {
                var count = store.Instances.Count;
                Assert.IsTrue(count > 0, "Should have more than zero instances");
                store.Close();
            }
        }

        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void IfcStoreSaveAndOpenIfcZipTest()
        {
            long count;
            //create a zip file using esent
            using (var store = IfcStore.Open("4walls1floorSite.ifc", null, 0))
            {
                count = store.Instances.Count;
                store.SaveAs("4walls1floorSite", IfcStorageType.IfcZip | IfcStorageType.Ifc);
                store.Close();
            }
            using (var store = IfcStore.Open("4walls1floorSite.ifczip", null, 0))
            {               
                Assert.IsTrue(count == store.Instances.Count, "Should have same number of instances");
                store.Close();
            }
            using (var store = IfcStore.Open("4walls1floorSite.ifc")) //now with memory model
            {
                count = store.Instances.Count;
                store.SaveAs("4walls1floorSite", IfcStorageType.IfcZip | IfcStorageType.Ifc);
                store.Close();
            }
            using (var store = IfcStore.Open("4walls1floorSite.ifczip"))
            {
                Assert.IsTrue(count == store.Instances.Count, "Should have same number of instances");
                store.Close();
            }
        }

         [TestMethod]
        [DeploymentItem("TestFiles")]
        public void IfcStoreSaveAndOpenIfcXmlZipTest()
        {
            long count;
            //create a zip file using esent
            using (var store = IfcStore.Open("4walls1floorSite.ifc", null, 0))
            {
                count = store.Instances.Count;
                store.SaveAs("4walls1floorSite", IfcStorageType.IfcZip | IfcStorageType.IfcXml);
                store.Close();
            }
            using (var store = IfcStore.Open("4walls1floorSite.ifczip", null, 0))
            {
                Assert.IsTrue(count == store.Instances.Count, "Should have same number of instances");
                store.Close();
            }
            using (var store = IfcStore.Open("4walls1floorSite.ifc")) //now with memory model
            {
                count = store.Instances.Count;
                store.SaveAs("4walls1floorSite", IfcStorageType.IfcZip | IfcStorageType.IfcXml);
                store.Close();
            }
            using (var store = IfcStore.Open("4walls1floorSite.ifczip"))
            {
                Assert.IsTrue(count == store.Instances.Count, "Should have same number of instances");
                store.Close();
            }
        }
         [TestMethod]
         [DeploymentItem("TestFiles")]
         public void IfcStoreSaveAndOpenIfcXml4Test()
         {
             long count;
             //create a zip file using esent
             using (var store = IfcStore.Open("SampleHouse4.ifc", null, 0))
             {
                 count = store.Instances.Count;
                 store.SaveAs("SampleHouse4",  IfcStorageType.IfcXml);
                 store.Close();
             }
             using (var store = IfcStore.Open("SampleHouse4.ifcxml", null, 0))
             {
                 Assert.IsTrue(count == store.Instances.Count, "Should have same number of instances");
                 store.Close();
             }
             using (var store = IfcStore.Open("SampleHouse4.ifc")) //now with memory model
             {
                 count = store.Instances.Count;
                 store.SaveAs("SampleHouse4",  IfcStorageType.IfcXml);
                 store.Close();
             }
             using (var store = IfcStore.Open("SampleHouse4.ifcxml"))
             {
                 Assert.IsTrue(count == store.Instances.Count, "Should have same number of instances");
                 store.Close();
             }
         }

        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void IfcStoreCreateStoreTest()
        {
            var credentials = new XbimEditorCredentials
            {
                ApplicationIdentifier = "XbimTest1",
                ApplicationDevelopersName = "Tester",
                EditorsOrganisationName = "XbimTeam"
            };

            using (var store = IfcStore.Create(credentials, IfcSchemaVersion.Ifc2X3,XbimStoreType.EsentDatabase))
            {
                using (var txn = store.BeginTransaction())
                {
                    var door = store.Instances.New<IfcDoor>();
                    door.Name = "Door 1";
                    txn.Commit();
                }
                store.SaveAs("esent2x3.ifc");
                store.Close();
            }
            using (var store = IfcStore.Create(credentials, IfcSchemaVersion.Ifc4, XbimStoreType.EsentDatabase))
            {
                using (var txn = store.BeginTransaction())
                {
                    var door = store.Instances.New<Ifc4.SharedBldgElements.IfcDoor>();
                    door.Name = "Door 1";
                    txn.Commit();
                }
                store.SaveAs("esent4.ifc");
                store.Close();
            }

            using (var store = IfcStore.Create(credentials, IfcSchemaVersion.Ifc2X3, XbimStoreType.InMemoryModel))
            {
                using (var txn = store.BeginTransaction())
                {
                    var door = store.Instances.New<IfcDoor>();
                    door.Name = "Door 1";
                    txn.Commit();
                }
                store.SaveAs("Memory2X3.ifc");
                store.Close();
            }

            using (var store = IfcStore.Create(credentials, IfcSchemaVersion.Ifc4, XbimStoreType.InMemoryModel))
            {
                using (var txn = store.BeginTransaction())
                {
                    var door = store.Instances.New<Ifc4.SharedBldgElements.IfcDoor>();
                    door.Name = "Door 1";
                    txn.Commit();
                }
                store.SaveAs("Memory4.ifc");
                store.Close();
            }

            var schemaVersion = IfcStore.GetIfcSchemaVersion("Esent2X3.ifc");
            Assert.IsTrue(schemaVersion == IfcSchemaVersion.Ifc2X3);
            schemaVersion = IfcStore.GetIfcSchemaVersion("Esent4.ifc");
            Assert.IsTrue(schemaVersion == IfcSchemaVersion.Ifc4);
            schemaVersion = IfcStore.GetIfcSchemaVersion("Memory2X3.ifc");
            Assert.IsTrue(schemaVersion == IfcSchemaVersion.Ifc2X3);
            schemaVersion = IfcStore.GetIfcSchemaVersion("Memory4.ifc");
            Assert.IsTrue(schemaVersion == IfcSchemaVersion.Ifc4);
        }

        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void ReadIfcHeaderTest()
        {
            var schemaVersion = IfcStore.GetIfcSchemaVersion("SampleHouse4.ifc");
            Assert.IsTrue(schemaVersion==IfcSchemaVersion.Ifc4);
            schemaVersion = IfcStore.GetIfcSchemaVersion("4walls1floorSite.ifc");
            Assert.IsTrue(schemaVersion==IfcSchemaVersion.Ifc2X3);

            //first run with a memory model opeing Ifc4 file
            long count;
            var credentials = new XbimEditorCredentials
            {
                ApplicationIdentifier = "XbimTest1",
                ApplicationDevelopersName = "Tester",
                EditorsOrganisationName = "XbimTeam"
            };
            using (var store = IfcStore.Open("SampleHouse4.ifc"))
            {
                count = store.Instances.Count;
                Assert.IsTrue(count > 0, "Should have more than zero instances");
                store.Close();
            }
            //create xbim files and check them , a size of 1MB will cause Esent to be used       
            using (var store = IfcStore.Open("SampleHouse4.ifc", credentials, 1.0))
            {
                Assert.IsTrue(count == store.Instances.Count, "Should have the same number of instances as the memory model");
                store.Close();
            }
            
            //now repeat for Ifc2x3
            using (var store = IfcStore.Open("4walls1floorSite.ifc"))
            {
                count = store.Instances.Count;
                Assert.IsTrue(count > 0, "Should have more than zero instances");
                store.Close();
            }
            //create xbim files and check them , a size of 1MB will cause Esent to be used       
            using (var store = IfcStore.Open("4walls1floorSite.ifc", null, 0.1))
            {
                Assert.IsTrue(count == store.Instances.Count, "Should have the same number of instances as the memory model");
                store.Close();
            }
        }

        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void IfcStoreTransactionTest()
        {
            using (var ifcStore = IfcStore.Open("4walls1floorSite.ifc")) //test memory model databases first
            {
                var count = ifcStore.Instances.Count;
                Assert.IsTrue(count > 0, "Should have more than zero instances"); //read mode is working
                using (var txn = ifcStore.BeginTransaction())
                {
                    var door = ifcStore.Instances.New<IfcDoor>();
                    door.Name = "Door 1";
                    txn.Commit();
                }
                Assert.IsTrue(ifcStore.Instances.Count == count +7); //door plus all the owner history objects
                ifcStore.SaveAs("4walls1floorSiteDoorMM.ifc");
                ifcStore.Close();
            }

            using (var ifcStore = IfcStore.Open("4walls1floorSite.ifc", null, 0)) //test esent databases first
            {
                var count = ifcStore.Instances.Count;
                Assert.IsTrue(count > 0, "Should have more than zero instances"); //read mode is working
                using (var txn = ifcStore.BeginTransaction())
                {
                    var door = ifcStore.Instances.New<IfcDoor>();
                    door.Name = "Door 1";
                    txn.Commit();
                }
                Assert.IsTrue(ifcStore.Instances.Count == count + 7); //door plus all the owner history objects
                ifcStore.SaveAs("4walls1floorSiteDoorES.ifc");
                ifcStore.Close();
            }


            var diff =
                File.ReadAllLines("4walls1floorSiteDoorES.ifc").Except(File.ReadAllLines("4walls1floorSiteDoorMM.ifc"));
            var enumerable = diff as string[] ?? diff.ToArray();
            Assert.IsTrue(!enumerable.Any() || (enumerable.Count() == 1 && enumerable.First().Contains("FILE_NAME"))); //might be a slight time difference in the timestamp of this line
        }

        [TestMethod]
        [DeploymentItem("TestFiles")]
        public void IfcStoreSaveAsXbimTest()
        {
            long originalCount;
            using (var ifcStore = IfcStore.Open("4walls1floorSite.ifc", null, 0)) //test esent databases first
            {
                var  count = originalCount = ifcStore.Instances.Count;
                Assert.IsTrue(count > 0, "Should have more than zero instances"); //read mode is working               
                ifcStore.SaveAs("4walls1floorSiteDoorES.xbim");
                ifcStore.Close();
            }

            using (var ifcStore = IfcStore.Open("4walls1floorSiteDoorES.xbim")) //test esent databases first
            {
                var count = ifcStore.Instances.Count;
                Assert.IsTrue(count > 0, "Should have more than zero instances"); //read mode is working                
                ifcStore.SaveAs("4walls1floorSiteDoorES2.Ifc");
                ifcStore.Close();
            }
            using (var ifcStore = IfcStore.Open("4walls1floorSiteDoorES2.ifc")) //test esent databases first
            {
                var count = ifcStore.Instances.Count;
                Assert.IsTrue(count == originalCount, "Should have more than zero instances"); //read mode is working                              
                ifcStore.Close();
            }
        }

        [TestMethod] [DeploymentItem("TestFiles")] 
        public void IfcStoreInitialisationTest()
        {
            var credentials = new XbimEditorCredentials
            {
                ApplicationIdentifier = "XbimTest1",
                ApplicationDevelopersName = "Tester",
                EditorsOrganisationName = "XbimTeam"
            };

            using (var store = IfcStore.Create(credentials, IfcSchemaVersion.Ifc4, XbimStoreType.EsentDatabase))
            {
                using (var txn = store.BeginTransaction())
                {
                    var project = store.Instances.New<Ifc4.Kernel.IfcProject>();
                    project.Initialize(ProjectUnits.SIUnitsUK);
                    txn.Commit();
                }
                store.SaveAs("esent4.ifc");
                store.Close();
            }

            using (var store = IfcStore.Create(credentials, IfcSchemaVersion.Ifc2X3, XbimStoreType.EsentDatabase))
            {
                using (var txn = store.BeginTransaction())
                {
                    var project = store.Instances.New<Ifc2x3.Kernel.IfcProject>();
                    project.Initialize(ProjectUnits.SIUnitsUK);
                    txn.Commit();
                }
                store.SaveAs("esent2x3.ifc");
                store.Close();
            }
        }

    }
}
