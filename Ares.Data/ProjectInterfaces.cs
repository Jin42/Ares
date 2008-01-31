﻿using System;
using System.Collections.Generic;

namespace Ares.Data
{

    /// <summary>
    /// A mode in the project.
    /// </summary>
    public interface IMode
    {
        /// <summary>
        /// Title of the mode.
        /// </summary>
        String Title { get; set; }

        /// <summary>
        /// Key which switches to the mode.
        /// </summary>
        Int32 KeyCode { get; set; }

        /// <summary>
        /// Adds a top-level element to the mode.
        /// </summary>
        void AddElement(IModeElement element);

        /// <summary>
        /// Removes an element from the mode.
        /// </summary>
        void RemoveElement(IModeElement element);

        /// <summary>
        /// Returns all elements in the mode.
        /// </summary>
        IList<IModeElement> GetElements();

        /// <summary>
        /// Returns whether a certain key triggers an element in this mode.
        /// </summary>
        bool ContainsKeyTrigger(Int32 keyCode);

        /// <summary>
        /// Returns the element which is triggered by a certain key.
        /// </summary>
        IModeElement GetTriggeredElement(Int32 keyCode);
    }

    /// <summary>
    /// Global Volumes which can be set.
    /// </summary>
    public enum VolumeTarget
    {
        /// <summary>
        /// Sound effects.
        /// </summary>
        Sounds = 0,
        /// <summary>
        /// Music files.
        /// </summary>
        Music = 1,
        /// <summary>
        /// Overall volume.
        /// </summary>
        Both = 2
    }

    /// <summary>
    /// Represents an ARES project.
    /// </summary>
    public interface IProject
    {
        /// <summary>
        /// Title of the project.
        /// </summary>
        String Title { get; set; }

        /// <summary>
        /// File where the project is stored.
        /// </summary>
        String FileName { get; set; }

        /// <summary>
        /// Whether the project has unsaved changes.
        /// </summary>
        bool Changed { get; set;  }

        /// <summary>
        /// Adds a mode to the project.
        /// </summary>
        IMode AddMode(String title);

        /// <summary>
        /// Removes a mode from the project.
        /// </summary>
        void RemoveMode(IMode mode);

        /// <summary>
        /// Returns all modes in the project.
        /// </summary>
        IList<IMode> GetModes();

        /// <summary>
        /// Returns whether the project contains a mode for a certain key.
        /// </summary>
        bool ContainsKeyMode(Int32 keyCode);

        /// <summary>
        /// Returns the mode for a certain key.
        /// </summary>
        IMode GetMode(Int32 keyCode);

        /// <summary>
        /// Returns a volume setting.
        /// </summary>
        Int32 GetVolume(VolumeTarget target);

        /// <summary>
        /// Sets a volume.
        /// </summary>
        void SetVolume(VolumeTarget target, Int32 value);
    }
}