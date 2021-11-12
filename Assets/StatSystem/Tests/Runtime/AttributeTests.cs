using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace StatSystem.Tests
{
    public class AttributeTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatSystem/Tests/Scenes/Test.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator Attribute_WhenModifierApplied_DoesNotExceedMaxValue()
        {
            yield return null;
            StatController statController = GameObject.FindObjectOfType<StatController>();
            Attribute health = statController.stats["Health"] as Attribute;
            Assert.AreEqual(100, health.currentValue);
            Assert.AreEqual(100, health.value);
            health.ApplyModifier(new StatModifier
            {
                magnitude = 20,
                type = ModifierOperationType.Additive
            });
            Assert.AreEqual(100, health.currentValue);
        }

        [UnityTest]
        [CanBeNull]
        public IEnumerator Attribute_WhenModifierApplied_DoesNotGoBelowZero()
        {
            yield return null;
            StatController statController = GameObject.FindObjectOfType<StatController>();
            Attribute health = statController.stats["Health"] as Attribute;
            Assert.AreEqual(100, health.currentValue);
            health.ApplyModifier(new StatModifier
            {
                magnitude = -150,
                type = ModifierOperationType.Additive
            });
            Assert.AreEqual(0, health.currentValue);
        }
    }
}