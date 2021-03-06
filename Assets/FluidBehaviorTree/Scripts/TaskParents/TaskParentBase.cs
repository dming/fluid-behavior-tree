﻿using System.Collections.Generic;
using Adnc.FluidBT.Tasks;
using Adnc.FluidBT.Trees;
using UnityEngine;

namespace Adnc.FluidBT.TaskParents {
    public abstract class TaskParentBase : ITaskParent {
        public BehaviorTree ParentTree { get; set; }
        public TaskStatus LastStatus { get; private set; }

        public string Name { get; set; }
        public bool Enabled { get; set; } = true;

        public List<ITask> Children { get; } = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;

        public GameObject Owner { get; set; }

        private int _lastTickCount;

        public TaskStatus Update () {
            UpdateTicks();

            var status = OnUpdate();
            LastStatus = status;

            return status;
        }

        private void UpdateTicks () {
            if (ParentTree == null) {
                return;
            }
            
            if (_lastTickCount != ParentTree.TickCount) {
                Reset();
            }

            _lastTickCount = ParentTree.TickCount;
        }

        public virtual void End () {
            throw new System.NotImplementedException();
        }

        protected virtual TaskStatus OnUpdate () {
            return TaskStatus.Success;
        }

        public virtual void Reset (bool hardReset = false) {
        }

        public virtual ITaskParent AddChild (ITask child) {
            if (!child.Enabled) {
                return this;
            }
            
            if (Children.Count < MaxChildren || MaxChildren < 0) {
                Children.Add(child);
            }

            return this;
        }
    }
}