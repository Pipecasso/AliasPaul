//#define NUnit                                               // Uncomment this when using NUnit instead of MSTest

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Telerik.JustMock;
using Telerik.JustMock.Expectations;
using Telerik.JustMock.Helpers;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
#if NUnit
using NUnit.Framework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Mocka
{
    /// <summary>
    /// A class to handle creation of mocked objects, static arrangements and the final assertions of all mocks.
    /// To use this class, create an instance of the class as a private member within the test class.
    /// Non-static mocking: Call Create&lt;classOrInterfaceToMock&gt;() to create each mock.
    /// Static mocking: Call ArrangeStatic(expression) to arrange a static call.
    /// Call AssertAll() at the end of every test - this class maintains lists of all mocks and ensures that the necessary asserts are called.
    /// This can significantly aid in the factoring of unit tests by making it much simpler to provide private arrange functions within the test source file
    /// that correspond to private methods called by the method under test.
    /// Note that all mocks are created with Constructor.Mocked/StaticConstructor.Mocked and Behavior.Strict by default.
    /// (Constructor.Mocked has no effect when mocking an interface, but is advisable when mocking a concrete class.)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class MockCreationHelper
	{
		public class CreatedMockObject
		{
			public object MockObject { get; private set; }
			public Type OriginalObjectType { get; private set; }
			public string StackTrace { get; private set; }

			public CreatedMockObject(object mockObject, Type originalObjectType)
			{
				MockObject = mockObject;
				OriginalObjectType = originalObjectType;
				// Lose 2 frames off the top and stop when the filename is unknown
				StackTrace stackTrace = new StackTrace(2, true);
				foreach (StackFrame stackFrame in stackTrace.GetFrames())
				{
					if (stackFrame.ToString().Contains("<filename unknown>"))
					{
						break;
					}
					StackTrace += "    at " + stackFrame.ToString();
				}
			}
		}

        public class StaticArrangement
        {
            public LambdaExpression Expression {get; private set;}
            public string StackTrace { get; private set; }

            public StaticArrangement(LambdaExpression expression)
            {
                Expression = expression;
                // Lose 2 frames off the top and stop when the filename is unknown
                StackTrace stackTrace = new StackTrace(2, true);
                foreach (StackFrame stackFrame in stackTrace.GetFrames())
                {
                    if (stackFrame.ToString().Contains("<filename unknown>"))
                    {
                        break;
                    }
                    StackTrace += "    at " + stackFrame.ToString();
                }
            }
        }

		private readonly List<CreatedMockObject> _createdMockObjects = new List<CreatedMockObject>();
        private readonly List<StaticArrangement> _staticArrangements = new List<StaticArrangement>();

        public IEnumerable<CreatedMockObject> CreatedMockObjects
        {
            get
            {
                return _createdMockObjects;
            }
        }

        public IEnumerable<StaticArrangement> StaticArrangements
        {
            get
            {
                return _staticArrangements;
            }
        }

		/// <summary>
		/// Creates a mock of the specified type, and adds it to the list of mocks for subsequent assert by AssertAll.
		/// </summary>
		/// <typeparam name="T">The type to be mocked</typeparam>
        /// <param name="constructor">Default is Mocked</param>
        /// <param name="behaviour">Default is Strict</param>
		/// <returns>The mocked instance of the specified type</returns>
        public T Create<T>(Constructor constructor = Constructor.Mocked, Behavior behaviour = Behavior.Strict)
		{
            T result = Mock.Create<T>(constructor, behaviour);
			_createdMockObjects.Add(new CreatedMockObject(result, typeof(T)));
			return result;
		}

        /// <summary>
        /// Creates a mock of the specified type, and adds it to the list of mocks for subsequent assert by AssertAll.
        /// </summary>
        /// <param name="type">The type to be mocked</param>
        /// <param name="constructor">Default is Mocked</param>
        /// <param name="behaviour">Default is Strict</param>
        /// <returns>The mocked instance of the specified type</returns>
        public object Create(Type type, Constructor constructor = Constructor.Mocked, Behavior behaviour = Behavior.Strict)
        {
            object result = Mock.Create(type, constructor, behaviour);
            _createdMockObjects.Add(new CreatedMockObject(result, type));
            return result;
        }

        /// <summary>
        /// Calls SetupStatic for the specified type.
        /// </summary>
        /// <param name="type">The type to set up static on</param>
        /// <param name="behaviour">The Behaviour value</param>
        /// <param name="staticConstructor">The StaticConstructor value</param>
        /// <remarks>Asserts if the call was made from a SetUp (NUnit) or TestInitialize (MSTest) method</remarks>
        public static void SetupStatic(Type type)
        {
#if NUnit
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(SetUpAttribute)),
                "Calling SetupStatic from the SetUp method doesn't work (known bug in JustMock)");
#else
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(TestInitializeAttribute)),
                "Calling SetupStatic from the TestInitialize method doesn't work (known bug in JustMock)");
#endif
            Mock.SetupStatic(type);
        }

        /// <summary>
        /// Calls SetupStatic for the specified type.
        /// </summary>
        /// <param name="type">The type to set up static on</param>
        /// <param name="behaviour">The Behaviour value</param>
        /// <remarks>Asserts if the call was made from a SetUp (NUnit) or TestInitialize (MSTest) method</remarks>
        public static void SetupStatic(Type type, Behavior behaviour)
        {
#if NUnit
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(SetUpAttribute)),
                "Calling SetupStatic from the SetUp method doesn't work (known bug in JustMock)");
#else
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(TestInitializeAttribute)),
                "Calling SetupStatic from the TestInitialize method doesn't work (known bug in JustMock)");
#endif
            Mock.SetupStatic(type, behaviour);
        }

        /// <summary>
        /// Calls SetupStatic for the specified type.
        /// </summary>
        /// <param name="type">The type to set up static on</param>
        /// <param name="staticConstructor">The StaticConstructor value</param>
        /// <remarks>Asserts if the call was made from a SetUp (NUnit) or TestInitialize (MSTest) method</remarks>
        public static void SetupStatic(Type type, StaticConstructor staticConstructor)
        {
#if NUnit
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(SetUpAttribute)),
                "Calling SetupStatic from the SetUp method doesn't work (known bug in JustMock)");
#else
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(TestInitializeAttribute)),
                "Calling SetupStatic from the TestInitialize method doesn't work (known bug in JustMock)");
#endif
            Mock.SetupStatic(type, staticConstructor);
        }

        /// <summary>
        /// Calls SetupStatic for the specified type.
        /// </summary>
        /// <param name="type">The type to set up static on</param>
        /// <param name="behaviour">The Behaviour value</param>
        /// <param name="staticConstructor">The StaticConstructor value</param>
        /// <remarks>Asserts if the call was made from a SetUp (NUnit) or TestInitialize (MSTest) method</remarks>
        public static void SetupStatic(Type type, Behavior behaviour, StaticConstructor staticConstructor)
        {
#if NUnit
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(SetUpAttribute)),
                "Calling SetupStatic from the SetUp method doesn't work (known bug in JustMock)");
#else
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(TestInitializeAttribute)),
                "Calling SetupStatic from the TestInitialize method doesn't work (known bug in JustMock)");
#endif
            Mock.SetupStatic(type, behaviour, staticConstructor);
        }

        /// <summary>
        /// Arranges a static method call. Arranges the expression and adds the expression to the list for subseqent assert by AssertAll()
        /// </summary>
        /// <param name="expression">The expression to be arranged, must be a static method</param>
        /// <returns>The ActionExpectation as returned by Mock.Arrange</returns>
        public ActionExpectation ArrangeStatic(Expression<Action> expression)
        {
            if (expression.Body is MethodCallExpression)
            {
                Assert.IsTrue(((MethodCallExpression)expression.Body).Method.IsStatic, "ArrangeStatic called for a non-static method");
            }
            else
            {
                Assert.IsTrue(((PropertyInfo)((MemberExpression)expression.Body).Member).GetGetMethod().IsStatic, "ArrangeStatic called for a non-static property");
            }
            _staticArrangements.Add(new StaticArrangement(expression));
            return Mock.Arrange(expression);
        }

        /// <summary>
        /// Arranges a static method call. Calls SetupStatic on the class, arranges the expression and adds the expression to the list for subseqent assert by AssertAll()
        /// </summary>
        /// <param name="expression">The expression to be arranged, must be a static method</param>
        /// <returns>The ActionExpectation as returned by Mock.Arrange</returns>
        public FuncExpectation<TResult> ArrangeStatic<TResult>(Expression<Func<TResult>> expression)
        {
            if (expression.Body is MethodCallExpression)
            {
                Assert.IsTrue(((MethodCallExpression)expression.Body).Method.IsStatic, "ArrangeStatic called for a non-static method");
            }
            else
            {
                Assert.IsTrue(((PropertyInfo)((MemberExpression)expression.Body).Member).GetGetMethod().IsStatic, "ArrangeStatic called for a non-static property");
            }
            _staticArrangements.Add(new StaticArrangement(expression));
            return Mock.Arrange(expression);
        }

		/// <summary>
		/// Calls AssertAll() on each of the mocks in the list.
		/// For easier analysis of failures, the stacktrace at creation is added to the failure message.
		/// </summary>
        /// <remarks>Asserts if the call was made from a SetUp (NUnit) or TestInitialize (MSTest) method</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Telerik.JustMock.Mock.Assert(System.Linq.Expressions.Expression<System.Action>,System.String)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Telerik.JustMock.Helpers.FluentHelper.AssertAll<System.Object>(T,System.String)")]
        public void AssertAll()
		{
#if NUnit
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(SetUpAttribute)),
                "Calling SetupStatic from the SetUp method doesn't work (known bug in JustMock)");
#else
            Assert.IsNull((new StackTrace()).GetFrame(1).GetMethod().GetCustomAttribute(typeof(TestInitializeAttribute)),
                "Calling SetupStatic from the TestInitialize method doesn't work (known bug in JustMock)");
#endif
            foreach (CreatedMockObject createdMockObject in _createdMockObjects)
			{
				createdMockObject.MockObject.AssertAll("AssertAll failed on object of type = " + createdMockObject.OriginalObjectType.ToString() + "\nStacktrace at object creation:\n" + createdMockObject.StackTrace);
			}
            foreach (StaticArrangement staticArrangement in _staticArrangements)
            {
                string typeName;
                if (staticArrangement.Expression.Body is MethodCallExpression)
                {
                    typeName = ((MethodCallExpression)(staticArrangement.Expression).Body).Method.DeclaringType.Name;
                }
                else
                {
                    typeName = ((PropertyInfo)((MemberExpression)staticArrangement.Expression.Body).Member).DeclaringType.Name;
                }
                if (staticArrangement.Expression.ReturnType != typeof(void))
                {
                    // It's a Func
                    MethodInfo assertHelperMethod = typeof(MockCreationHelper).GetMethod("AssertHelper", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    MethodInfo assertHelperWithTypeMethod = assertHelperMethod.MakeGenericMethod(staticArrangement.Expression.ReturnType);
                    try
                    {
                        assertHelperWithTypeMethod.Invoke(this, new object[] { staticArrangement, typeName });
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }
                else
                {
                    // It's an Action
                    Mock.Assert((Expression<Action>)staticArrangement.Expression, "Static assert failed on type " + typeName + "\nStacktrace at arrangement:\n" + staticArrangement.StackTrace);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Telerik.JustMock.Mock.Assert<System.Linq.Expressions.LambdaExpression>(T,System.String)")]
        private static void AssertHelper<T>(StaticArrangement staticArrangement, string typeName)
        {
            Mock.Assert(staticArrangement.Expression, "Static assert failed on type " + typeName + "\nStacktrace at object creation:\n" + staticArrangement.StackTrace);
        }
	}
}
