﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#region Using Directives
using System;
using Nav.Language.Tests.Regression.Test1.IWFL;
using Pharmatechnik.Apotheke.XTplus.Common.WFL;
using Pharmatechnik.Apotheke.XTplus.Common.IWFL;
using Pharmatechnik.Apotheke.XTplus.Framework.Core.WFL;
using Pharmatechnik.Apotheke.XTplus.Framework.Core.IWFL;
using Pharmatechnik.Apotheke.XTplus.Framework.NavigationEngine.WFL;
using Pharmatechnik.Apotheke.XTplus.Framework.NavigationEngine.IWFL;
#endregion

namespace Nav.Language.Tests.Regression.Test1.WFL {
    #region Nav Annotations
    /// <NavFile>..\..\ConcatSample.nav</NavFile>
    /// <NavTask>ConcatSample</NavTask>
    #endregion
    public abstract partial class ConcatSampleWFSBase: StandardWFS {

        readonly NS.A.WFL.IBeginAWFS _a;
        readonly NS.B.WFL.IBeginBWFS _b;
        readonly NS.C.WFL.IBeginCWFS _c;

        public ConcatSampleWFSBase(Pharmatechnik.Apotheke.XTplus.Framework.NavigationEngine.IWFL.IClientSideWFS clientSideWFS) {}

        public ConcatSampleWFSBase(NS.A.WFL.IBeginAWFS a,
                                   NS.B.WFL.IBeginBWFS b,
                                   NS.C.WFL.IBeginCWFS c) {
            _a = a;
            _b = b;
            _c = c;
        }

        protected virtual ViewTO BeforeTriggerLogic(ViewTO to) => to;

        #region Nav Annotations
        /// <NavInit>Init1</NavInit>
        #endregion
        public virtual IINIT_TASK Begin() {
            var body = BeginLogic();
            switch(body) {
                case ViewTO viewTO:
                    return GotoGUI(viewTO);
                case CANCEL cancel:
                    return cancel;
                default:
                    throw new InvalidOperationException(NavCommandBody.ComposeUnexpectedTransitionMessage(nameof(BeginLogic), body));
            }
        }

        #region Nav Annotations
        /// <NavInit>Init1</NavInit>
        #endregion
        protected abstract INavCommandBody BeginLogic();

        protected sealed record Init2CallContext(ConcatSampleWFSBase wfs) {

            internal Continuation Show(ViewTO to) => new (wfs, to);

            internal sealed record Continuation(ConcatSampleWFSBase wfs, ViewTO to) {

                public INavCommandBody BeginA() {
                    return new ConcatCommand {
                        TO           = to,
                        Continuation = wfs.OpenModalTask<FooResult>(() => wfs._a.Begin(), wfs.AfterA),
                    };
                }

                public INavCommandBody BeginA(string a1) {
                    return new ConcatCommand {
                        TO           = to,
                        Continuation = wfs.OpenModalTask<FooResult>(() => wfs._a.Begin(a1), wfs.AfterA),
                    };
                }

            }
        }

        #region Nav Annotations
        /// <NavInit>Init2</NavInit>
        #endregion
        public virtual IINIT_TASK Begin(string message) {
            var callContext = new Init2CallContext(this);
            var body = BeginLogic(message, callContext);
            switch(body) {
                case ViewTO viewTO:
                    return GotoGUI(viewTO);
                case ConcatCommand concatCommand when concatCommand.TO is ViewTO to:
                    return GotoGUI(to).Concat(concatCommand.Continuation);
                case CANCEL cancel:
                    return cancel;
                default:
                    throw new InvalidOperationException(NavCommandBody.ComposeUnexpectedTransitionMessage(nameof(BeginLogic), body));
            }
        }

        #region Nav Annotations
        /// <NavInit>Init2</NavInit>
        #endregion
        protected abstract INavCommandBody BeginLogic(string message,
                                                      Init2CallContext callContext);

        #region Nav Annotations
        /// <NavExit>A</NavExit>
        #endregion
        private INavCommand AfterA(FooResult result) {
            var body = AfterALogic(result);
            switch(body) {
                case ViewTO viewTO:
                    return GotoGUI(viewTO);
                case CANCEL cancel:
                    return cancel;
                default:
                    throw new InvalidOperationException(NavCommandBody.ComposeUnexpectedTransitionMessage(nameof(AfterALogic), body));
            }
        }

        #region Nav Annotations
        /// <NavExit>A</NavExit>
        #endregion
        protected abstract INavCommandBody AfterALogic(FooResult result);

        protected sealed record AfterBCallContext(ConcatSampleWFSBase wfs) {

            internal Continuation Show(ViewTO to) => new (wfs, to);

            internal sealed record Continuation(ConcatSampleWFSBase wfs, ViewTO to) {

                public INavCommandBody BeginC() {
                    return new ConcatCommand {
                        TO           = to,
                        Continuation = wfs.OpenModalTask<FooResult>(() => wfs._c.Begin(), wfs.AfterC),
                    };
                }

                public INavCommandBody BeginC(string c1) {
                    return new ConcatCommand {
                        TO           = to,
                        Continuation = wfs.OpenModalTask<FooResult>(() => wfs._c.Begin(c1), wfs.AfterC),
                    };
                }

            }
        }

        #region Nav Annotations
        /// <NavExit>B</NavExit>
        #endregion
        private INavCommand AfterB(FooResult result) {
            var callContext = new AfterBCallContext(this);
            var body = AfterBLogic(result, callContext);
            switch(body) {
                case ConcatCommand concatCommand when concatCommand.TO is ViewTO to:
                    return GotoGUI(to).Concat(concatCommand.Continuation);
                case CANCEL cancel:
                    return cancel;
                default:
                    throw new InvalidOperationException(NavCommandBody.ComposeUnexpectedTransitionMessage(nameof(AfterBLogic), body));
            }
        }

        #region Nav Annotations
        /// <NavExit>B</NavExit>
        #endregion
        protected abstract INavCommandBody AfterBLogic(FooResult result,
                                                       AfterBCallContext callContext);

        #region Nav Annotations
        /// <NavExit>C</NavExit>
        #endregion
        private INavCommand AfterC(FooResult result) {
            var body = AfterCLogic(result);
            switch(body) {
                case TASK_RESULT taskResult:
                    return taskResult;
                case CANCEL cancel:
                    return cancel;
                default:
                    throw new InvalidOperationException(NavCommandBody.ComposeUnexpectedTransitionMessage(nameof(AfterCLogic), body));
            }
        }

        #region Nav Annotations
        /// <NavExit>C</NavExit>
        #endregion
        protected abstract INavCommandBody AfterCLogic(FooResult result);

        protected sealed record OnFooCallContext(ConcatSampleWFSBase wfs) {

            internal Continuation Show(ViewTO to) => new (wfs, to);

            internal sealed record Continuation(ConcatSampleWFSBase wfs, ViewTO to) {

                public INavCommandBody BeginB() {
                    return new ConcatCommand {
                        TO           = to,
                        Continuation = wfs.OpenModalTask<FooResult>(() => wfs._b.Begin(), wfs.AfterB),
                    };
                }

                public INavCommandBody BeginB(string b1) {
                    return new ConcatCommand {
                        TO           = to,
                        Continuation = wfs.OpenModalTask<FooResult>(() => wfs._b.Begin(b1), wfs.AfterB),
                    };
                }

            }
        }

        #region Nav Annotations
        /// <NavTrigger>OnFoo</NavTrigger>
        #endregion
        public virtual INavCommand OnFoo(ViewTO to) {
            to = BeforeTriggerLogic(to);
            var callContext = new OnFooCallContext(this);
            var body = OnFooLogic(to, callContext);    
            switch(body) {
                case ViewTO viewTO:
                    return GotoGUI(viewTO);
                case ConcatCommand concatCommand when concatCommand.TO is ViewTO to:
                    return GotoGUI(to).Concat(concatCommand.Continuation);
                case CANCEL cancel:
                    return cancel;
                default:
                    throw new InvalidOperationException(NavCommandBody.ComposeUnexpectedTransitionMessage(nameof(OnFooLogic), body));
            }
        }

        #region Nav Annotations
        /// <NavTrigger>OnFoo</NavTrigger>
        #endregion
        protected abstract INavCommandBody OnFooLogic(ViewTO to,
                                                      OnFooCallContext callContext);

        protected INavCommandBody TaskResult(bool par) {
            return InternalTaskResult(par);
        }

    }

    #region Nav Annotations
    /// <NavFile>..\..\ConcatSample.nav</NavFile>
    /// <NavTask>ConcatSample</NavTask>
    #endregion
    public partial class ConcatSampleWFS: ConcatSampleWFSBase, IConcatSampleWFS, IBeginConcatSampleWFS {

        readonly ITestBS _testBS;
        readonly ISozFactory _SOZFactory;

        public ConcatSampleWFS(Pharmatechnik.Apotheke.XTplus.Framework.NavigationEngine.IWFL.IClientSideWFS clientSideWFS): base(clientSideWFS) {}

        public ConcatSampleWFS(NS.A.WFL.IBeginAWFS a,
                               NS.B.WFL.IBeginBWFS b,
                               NS.C.WFL.IBeginCWFS c,
                               ITestBS testBS,
                               ISozFactory SOZFactory)
            :base(a,
                  b,
                  c) {
            _testBS = testBS;
            _SOZFactory = SOZFactory;
        }
    }
}