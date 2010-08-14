﻿/*
 Copyright (c) 2010 [Joerg Ruedenauer]
 
 This file is part of Ares.

 Ares is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 Ares is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with Ares; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ares.Data;

namespace Ares.Editor.Actions
{
    public class ElementVolumeChangeAction : Action
    {
        public ElementVolumeChangeAction(IElement element, bool setsMusic, bool setsSound, int music, int sound)
        {
            m_Element = element;

            m_OldSetsMusic = element.SetsMusicVolume;
            m_OldSetsSound = element.SetsSoundVolume;
            m_OldMusic = element.MusicVolume;
            m_OldSound = element.SoundVolume;

            m_NewMusic = music;
            m_NewSetsMusic = setsMusic;
            m_NewSetsSound = setsSound;
            m_NewSound = sound;
        }

        public void SetData(bool setsMusic, bool setsSound, int music, int sound)
        {
            m_NewSound = sound;
            m_NewMusic = music;
            m_NewSetsSound = setsSound;
            m_NewSetsMusic = setsMusic;
        }

        public IElement Element
        {
            get
            {
                return m_Element;
            }
        }

        public override void Do()
        {
            m_Element.SetsMusicVolume = m_NewSetsMusic;
            m_Element.MusicVolume = m_NewMusic;
            m_Element.SetsSoundVolume = m_NewSetsSound;
            m_Element.SoundVolume = m_NewSound;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        public override void Undo()
        {
            m_Element.SetsMusicVolume = m_OldSetsMusic;
            m_Element.SetsSoundVolume = m_OldSetsSound;
            m_Element.MusicVolume = m_OldMusic;
            m_Element.SoundVolume = m_OldSound;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        private IElement m_Element;

        private bool m_OldSetsMusic;
        private bool m_OldSetsSound;
        private int m_OldMusic;
        private int m_OldSound;

        private bool m_NewSetsMusic;
        private bool m_NewSetsSound;
        private int m_NewMusic;
        private int m_NewSound;
    }

    public class ElementEffectsChangeAction : Action
    {
        public ElementEffectsChangeAction(IFileElement element, int volume)
        {
            m_Element = element;
            m_OldVolume = m_Element.Effects.Volume;
            m_NewVolume = volume;
        }

        public IFileElement Element
        {
            get
            {
                return m_Element;
            }
        }

        public void SetData(int volume)
        {
            m_NewVolume = volume;
        }

        public override void Do()
        {
            m_Element.Effects.Volume = m_NewVolume;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        public override void Undo()
        {
            m_Element.Effects.Volume = m_OldVolume;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        private IFileElement m_Element;
        private int m_OldVolume;
        private int m_NewVolume;
    }

    public class ElementRenamedAction : Action
    {
        public ElementRenamedAction(IElement element, String newName)
        {
            m_Element = element;
            m_OldName = element.Title;
            m_NewName = newName;
        }

        public IElement Element { get { return m_Element; } }

        public void SetName(String newName)
        {
            m_NewName = newName;
        }

        public override void Do()
        {
            m_Element.Title = m_NewName;
            ElementChanges.Instance.ElementRenamed(m_Element.Id);
        }

        public override void Undo()
        {
            m_Element.Title = m_OldName;
            ElementChanges.Instance.ElementRenamed(m_Element.Id);
        }

        private IElement m_Element;
        private String m_OldName;
        private String m_NewName;
    }

    public class DelayableElementChangeAction : Action
    {
        public DelayableElementChangeAction(IDelayableElement element, int fixedDelay, int maxDelay)
        {
            m_Element = element;
            m_OldFixed = element.FixedStartDelay;
            m_OldMax = element.MaximumRandomStartDelay;
            m_NewFixed = TimeSpan.FromMilliseconds(fixedDelay);
            m_NewMax = TimeSpan.FromMilliseconds(maxDelay);
        }

        public IDelayableElement Element
        {
            get
            {
                return m_Element;
            }
        }

        public void SetData(int fixedDelay, int maxDelay)
        {
            m_NewFixed = TimeSpan.FromMilliseconds(fixedDelay);
            m_NewMax = TimeSpan.FromMilliseconds(maxDelay);
        }

        public override void Do()
        {
            m_Element.FixedStartDelay = m_NewFixed;
            m_Element.MaximumRandomStartDelay = m_NewMax;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        public override void Undo()
        {
            m_Element.FixedStartDelay = m_OldFixed;
            m_Element.MaximumRandomStartDelay = m_OldMax;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        private IDelayableElement m_Element;
        private TimeSpan m_OldFixed;
        private TimeSpan m_NewFixed;
        private TimeSpan m_OldMax;
        private TimeSpan m_NewMax;
    }

    public class RepeatableElementChangeAction : Action
    {
        public RepeatableElementChangeAction(IRepeatableElement element, int repeatCount, int fixedDelay, int maxDelay)
        {
            m_Element = element;
            m_OldRepeatCount = element.RepeatCount;
            m_OldFixed = element.FixedIntermediateDelay;
            m_OldMax = element.MaximumRandomIntermediateDelay;
            m_NewRepeatCount = repeatCount;
            m_NewFixed = TimeSpan.FromMilliseconds(fixedDelay);
            m_NewMax = TimeSpan.FromMilliseconds(maxDelay);
        }

        public IRepeatableElement Element
        {
            get
            {
                return m_Element;
            }
        }

        public void SetData(int repeatCount, int fixedDelay, int maxDelay)
        {
            m_NewFixed = TimeSpan.FromMilliseconds(fixedDelay);
            m_NewMax = TimeSpan.FromMilliseconds(maxDelay);
            m_NewRepeatCount = repeatCount;
        }

        public override void Do()
        {
            m_Element.RepeatCount = m_NewRepeatCount;
            m_Element.FixedIntermediateDelay = m_NewFixed;
            m_Element.MaximumRandomIntermediateDelay = m_NewMax;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        public override void Undo()
        {
            m_Element.RepeatCount = m_OldRepeatCount;
            m_Element.FixedIntermediateDelay = m_OldFixed;
            m_Element.MaximumRandomIntermediateDelay = m_OldMax;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        private IRepeatableElement m_Element;
        private int m_OldRepeatCount;
        private int m_NewRepeatCount;

        private TimeSpan m_OldFixed;
        private TimeSpan m_NewFixed;
        private TimeSpan m_OldMax;
        private TimeSpan m_NewMax;
    }

    public class ChoiceElementChangeAction : Action
    {
        public ChoiceElementChangeAction(IChoiceElement element, int chance)
        {
            m_Element = element;
            m_OldChance = element.RandomChance;
            m_NewChance = chance;
        }

        public override void Do()
        {
            m_Element.RandomChance = m_NewChance;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        public override void Undo()
        {
            m_Element.RandomChance = m_OldChance;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        private IChoiceElement m_Element;
        private int m_OldChance;
        private int m_NewChance;
    }

    public class SequentialElementChangeAction : Action
    {
        public SequentialElementChangeAction(ISequentialElement element, int newFixed, int newRandom)
        {
            m_Element = element;
            m_OldFixed = element.FixedStartDelay;
            m_OldRandom = element.MaximumRandomStartDelay;
            m_NewFixed = TimeSpan.FromMilliseconds(newFixed);
            m_NewRandom = TimeSpan.FromMilliseconds(newRandom);
        }

        public override void Do()
        {
            m_Element.FixedStartDelay = m_NewFixed;
            m_Element.MaximumRandomStartDelay = m_NewRandom;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        public override void Undo()
        {
            m_Element.FixedStartDelay = m_OldFixed;
            m_Element.MaximumRandomStartDelay = m_OldRandom;
            ElementChanges.Instance.ElementChanged(m_Element.Id);
        }

        private ISequentialElement m_Element;
        private TimeSpan m_OldFixed;
        private TimeSpan m_NewFixed;
        private TimeSpan m_OldRandom;
        private TimeSpan m_NewRandom;
    }

    public class AddContainerElementsAction : Action
    {
        public AddContainerElementsAction(IGeneralElementContainer container, IList<IElement> elements)
        {
            m_Container = container;
            m_Elements = new List<IElement>();
            foreach (IElement element in elements)
            {
                IElement newElement = container.AddGeneralElement(element);
                m_Elements.Add(newElement);
                container.RemoveElement(newElement.Id);
            }
        }

        public override void Do()
        {
            int index = m_Container.GetGeneralElements().Count;
            foreach (IElement element in m_Elements)
            {
                m_Container.InsertGeneralElement(index, element);
                ++index;
            }
            ElementChanges.Instance.ElementChanged(m_Container.Id);
        }

        public override void Undo()
        {
            foreach (IElement element in m_Elements)
            {
                m_Container.RemoveElement(element.Id);
                ElementRemoval.NotifyRemoval(element);
            }
            ElementChanges.Instance.ElementChanged(m_Container.Id);
        }

        private IGeneralElementContainer m_Container;
        private List<IElement> m_Elements;
    }

    public class RemoveContainerElementsAction : Action
    {
        public RemoveContainerElementsAction(IGeneralElementContainer container, IList<IElement> elements, int index)
        {
            m_Container = container;
            m_Elements = elements;
            m_Index = index;
        }

        public override void Do()
        {
            foreach (IElement element in m_Elements)
            {
                m_Container.RemoveElement(element.Id);
                ElementRemoval.NotifyRemoval(element);
            }
            ElementChanges.Instance.ElementChanged(m_Container.Id);
        }

        public override void Undo()
        {
            int index = m_Index;
            foreach (IElement element in m_Elements)
            {
                m_Container.InsertGeneralElement(index, element);
                ++index;
            }
            ElementChanges.Instance.ElementChanged(m_Container.Id);
        }

        private IGeneralElementContainer m_Container;
        private IList<IElement> m_Elements;
        private int m_Index;
    }

    public class SetModeElementTriggerAction : Action
    {
        public SetModeElementTriggerAction(IModeElement modeElement, ITrigger trigger)
        {
            m_ModeElement = modeElement;
            m_OldTrigger = modeElement.Trigger;
            m_NewTrigger = trigger;
        }

        public override void Do()
        {
            m_ModeElement.Trigger = m_NewTrigger;
            ElementChanges.Instance.ElementTriggerChanged(m_ModeElement.Id);
            Ares.ModelInfo.ModelChecks.Instance.Check(ModelInfo.CheckType.Key);
        }

        public override void Undo()
        {
            m_ModeElement.Trigger = m_OldTrigger;
            ElementChanges.Instance.ElementTriggerChanged(m_ModeElement.Id);
            Ares.ModelInfo.ModelChecks.Instance.Check(ModelInfo.CheckType.Key);
        }

        private IModeElement m_ModeElement;
        private ITrigger m_OldTrigger;
        private ITrigger m_NewTrigger;
    }

}