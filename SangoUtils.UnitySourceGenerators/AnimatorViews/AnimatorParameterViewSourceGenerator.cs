using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using SangoUtils.UnitySourceGenerators.Utils;

namespace SangoUtils.UnitySourceGenerators.AnimatorViews
{
//    [Generator]
//    internal sealed class AnimatorParameterViewSourceGenerator : ISourceGenerator
//    {
//        public const string AnimatorParameterViewAttributeName = "AnimatorParameterView";
//        public const string AnimatorStateViewAttributeName = "AnimatorStateView";

//        public void Initialize(GeneratorInitializationContext context)
//        {
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif
//            context.RegisterForSyntaxNotifications(delegate { return new AnimatorViewSyntaxReceiver(); });
//        }

//        public void Execute(GeneratorExecutionContext context)
//        {
//            string AnimatorParameterViewAttributeSourceText = Def.Dom_Declaration +
//$@"
//using System;

//namespace {Def.Dom_Generateds}
//{{
//    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
//    public sealed class {AnimatorParameterViewAttributeName}Attribute : Attribute
//    {{ 
//        public string AnimatorPath {{ get; }}
        
//        public  {AnimatorParameterViewAttributeName}Attribute(string animatorPath)
//        {{
//            AnimatorPath = animatorPath;
//        }}
//    }}
//}}
//";

//            string AnimatorStateViewAttributeSourceText = Def.Dom_Declaration +
//$@"
//using System;

//namespace {Def.Dom_Generateds}
//{{
//    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
//    public sealed class {AnimatorStateViewAttributeName}Attribute : Attribute
//    {{ 
//        public string AnimatorPath {{ get; }}
        
//        public  {AnimatorStateViewAttributeName}Attribute(string animatorPath)
//        {{
//            AnimatorPath = animatorPath;
//        }}
//    }}
//}}
//";

//        }

       
//    }
}
