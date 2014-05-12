/*
	Copyright 2010 MCSharp Team Licensed under the
	Educational Community License, Version 2.0 (the "License"); you may
	not use this file except in compliance with the License. You may
	obtain a copy of the License at
	
	http://www.osedu.org/licenses/ECL-2.0
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the License for the specific language governing
	permissions and limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;

namespace FoxyMC
{
    public static class AssemblyLoader
    {
        public static Assembly scriptsAsm;
        public static bool needsRecompile = true;

        public static List<Type> GetAllClasses(string nameSpace)
        {
            if (scriptsAsm == null) scriptsAsm = Assembly.GetExecutingAssembly();
            nameSpace = "FoxyMC." + nameSpace;
            List<Type> namespaceList = new List<Type>();
            foreach (Type type in scriptsAsm.GetTypes())
            {
                if (type.Namespace == nameSpace)
                    namespaceList.Add(type);
            }
            return namespaceList;
        }

        /*public static void BuildScripts()
        {
            string dir = Directory.GetCurrentDirectory();
            string assembly = dir + "/FoxyMC_Scripts.dll";
            bool compSucc = false;
            try
            {
                if (needsRecompile)
                {
                    Server.s.Log("Script re-compile required!");
                    CodeDomProvider provider = new CSharpCodeProvider();
                    CompilerParameters compilerParms = new CompilerParameters
                    {
                        CompilerOptions = "/target:library",
                        GenerateExecutable = true,
                        GenerateInMemory = false,
                        IncludeDebugInformation = false,
                        OutputAssembly = assembly
                    };
                    compilerParms.ReferencedAssemblies.Add("System.Drawing.dll");
                    compilerParms.ReferencedAssemblies.Add("System.dll");
                    compilerParms.ReferencedAssemblies.Add(dir + "/FoxyMC.dll");
                    compilerParms.ReferencedAssemblies.Add("mscorlib.dll");



                    Directory.SetCurrentDirectory("scripts/");

                    string s = Directory.GetCurrentDirectory();

                    string[] files = Directory.GetFiles(".", "*.cs", SearchOption.AllDirectories);

                    CompilerResults cresult = provider.CompileAssemblyFromFile(compilerParms, files);

                    Directory.SetCurrentDirectory(dir);

                    if (cresult.Errors.HasErrors)
                    {
                        string errorMsg = "";
                        errorMsg = cresult.Errors.Count.ToString() + " Errors:";

                        for (int i = 0; i < cresult.Errors.Count; ++i)
                        {
                            errorMsg += "\r\nLine:" +
                            cresult.Errors[i].Line.ToString() + " - " +
                            cresult.Errors[i].ErrorText;
                        }
                        throw new Exception(errorMsg);
                    }
                    Server.s.Log("Script re-compile successful!");
                    compSucc = true;
                }
            }
            catch (Exception e) { Server.s.Log("ERROR compiling scripts assembly: " + e.ToString() + "!"); }
            if (needsRecompile || scriptsAsm == null)
            {
                scriptsAsm = Assembly.Load(File.ReadAllBytes(assembly));
                if (compSucc) needsRecompile = false;
            }
        }*/
    }
}
