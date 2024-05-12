using Chronus.Tags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.Utils
{
    public static class GameObjectExtension
    {
        public static bool CompareTag(this Component gameObject, TagType tagType)
        {
            return gameObject.CompareTag(tagType.ToString("F"));
        }
    }
}

