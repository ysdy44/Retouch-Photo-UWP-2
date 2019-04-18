﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.Library
{
    /// <summary> Drawing and managing Bezier curves on CanvasControl </summary>
    public class CurveNodes
    {
        readonly Color Blue = Color.FromArgb(255, 54, 135, 230);
        readonly Color TranslucentBlue = Color.FromArgb(90, 54, 135, 230);
        readonly Color White = Color.FromArgb(255, 255, 255, 255);
        readonly Color Shadow = Color.FromArgb(70, 127, 127, 127);

        private Node Node;

        public NodeEditMode EditMode = NodeEditMode.Add;
        public NodeControlMode ControlMode = NodeControlMode.Mirrored;

        

        #region Draw


        /// <summary>
        /// draw the curve
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="ds"></param>
        public void Draw( ICanvasResourceCreator creator, CanvasDrawingSession ds,Matrix3x2 matrix, List<Node> nodes)
        {
            if (nodes == null) return;
            if (nodes.Count == 0) return;

            //Transform
            foreach (Node item in nodes)
            {
                item.Transform(matrix);
            }

            //Geometry 
            CanvasGeometry geometry = CreatePath(creator, nodes);
            ds.DrawGeometry(geometry, Blue, 2);

            //Nodes
            foreach (Node item in nodes)
            {
                if (item.ChooseMode == NodeChooseMode.None) this.DrawNodeVector(ds, item.VectorTransform);
                else
                {
                    this.DrawNodeControl(ds, item.VectorTransform, item.RightControlTransform);
                    this.DrawNodeControl(ds, item.VectorTransform, item.LeftControlTransform);

                    this.DrawNodeVector2(ds, item.VectorTransform);
                }
            }

            //RectChoose
            if (this.EditMode.HasFlag(NodeEditMode.RectChoose))
            {
                Rect rect = new Rect(this.RectChooseStart.ToPoint(), this.RectChooseEnd.ToPoint());
                ds.FillRectangle(rect, this.TranslucentBlue);
                ds.DrawRectangle(rect, this.Blue);
            }
        }


        /// <summary>
        /// get geometry
        /// </summary>
        private CanvasGeometry CreatePath(ICanvasResourceCreator creator, List<Node> nodes)
        {
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(creator);
            pathBuilder.BeginFigure(nodes.First().VectorTransform);

            for (int i = 0; i < nodes.Count - 1; i++)//0 to 9
            {
                if (nodes[i].IsSmooth == false)
                {
                    pathBuilder.AddLine(nodes[i + 1].VectorTransform);
                }
                else
                {
                    pathBuilder.AddCubicBezier(nodes[i].LeftControlTransform, nodes[i + 1].RightControlTransform, nodes[i + 1].VectorTransform);
                }
            }

            pathBuilder.EndFigure(CanvasFigureLoop.Open);
            return CanvasGeometry.CreatePath(pathBuilder);
        }



        /// <summary>
        /// draw a ⊙
        /// </summary>
        private void DrawNodeVector(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, this.Shadow);
            ds.FillCircle(vector, 8, this.Blue);
            ds.FillCircle(vector, 6, this.White);
        }
        /// <summary>
        /// draw a ●
        /// </summary>
        private void DrawNodeVector2(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, this.Shadow);
            ds.FillCircle(vector, 8, this.White);
            ds.FillCircle(vector, 6, this.Blue);
        }
        /// <summary>
        /// draw a ——⊕ 
        /// </summary>
        private void DrawNodeControl(CanvasDrawingSession ds, Vector2 vector, Vector2 control)
        {
            ds.DrawLine(control, vector, this.Blue, 1);

            ds.FillCircle(control, 8, this.Shadow);
            ds.FillCircle(control, 7, this.Blue);
            ds.FillCircle(control, 6, this.White);
        }


        #endregion

        #region function


        /// <summary>
        /// Interpolation: Insert a new point between the selected points
        /// 插值：在选定的点之间插入新的点
        /// </summary>
        public void Interpolation( List<Node> nodes)
        {
            NodeChooseMode isChecked = NodeChooseMode.None;

            List<int> list = new List<int> { };

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ChooseMode != NodeChooseMode.None && isChecked != NodeChooseMode.None)
                {
                    list.Add(i);
                }

                isChecked = nodes[i].ChooseMode;
            }

            for (int i = 0; i < list.Count; i++)
            {
                int index = list[i] + i;
                Vector2 vect = nodes[index - 1].Vector + nodes[index].Vector;

                Node node = new Node(vect / 2);
                node.ChooseMode = NodeChooseMode.Vector;
                nodes.Insert(index, node);
            }
        }
        /// <summary>
        /// Remove: Removes all selected points
        /// 移除：移除所有选定的点
        /// </summary>
        public void Remove(List<Node> nodes)
        {
            IEnumerable<int> enumerable =
                from i in nodes
                where i.ChooseMode != NodeChooseMode.None
                select nodes.IndexOf(i);

            foreach (int item in enumerable.Reverse())
            {
                nodes.RemoveAt(item);
            }
        }


        /// <summary>
        /// Sharpening: Making the control line between points into a straight line
        /// 锐化：使点之间的控制线变成直线
        /// </summary>
        public void Sharp(List<Node> nodes)
        {
            foreach (Node item in nodes)
            {
                if (item.ChooseMode != NodeChooseMode.None)
                {
                    item.IsSmooth = false;

                    item.LeftControl = item.RightControl = item.Vector;
                }
            }
        }
        /// <summary>
        /// Smoothing: Makes the connection between the selected points into a Bezier curve
        /// 平滑：使选定的点之间的连线变成贝塞尔曲线
        /// </summary>
        public void Smooth(List<Node> nodes)
        {
            for (int i = 1; i < nodes.Count - 1; i++)
            {
                if (nodes[i].ChooseMode != NodeChooseMode.None)
                {
                    nodes[i].IsSmooth = true;

                    Vector2 Space = nodes[i + 1].Vector - nodes[i - 1].Vector;
                    Vector2 Spa = Space / 6;

                    nodes[i].LeftControl = nodes[i].Vector + Spa;
                    nodes[i].RightControl = nodes[i].Vector - Spa;
                }
            }
        }


        #endregion



        #region Operator


        public void Operator_Start(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            if (this.EditMode == NodeEditMode.Add)
            {
                Add_Start(vector, matrix, inverseMatrix, nodes);
            }
            else if (this.EditMode.HasFlag(NodeEditMode.EditMove))
            {
                this.Node = null;

                foreach (Node item in nodes)
                {
                    NodeChooseMode ChooseMode = item.GetChooseMode(vector);
                    if (ChooseMode != NodeChooseMode.None)
                    {
                        item.ChooseMode = ChooseMode;
                        this.Node = item;
                        break;
                    }
                }

                if (this.Node != null) EditMove_Start(vector, matrix, inverseMatrix, nodes);
                else RectChoose_Start(vector, matrix, inverseMatrix);
            }
        }

        public void Operator_Delta(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            if (this.EditMode == NodeEditMode.Add)
            {
                Add_Delta(vector, matrix, inverseMatrix, nodes);
            }
            else if (this.EditMode.HasFlag(NodeEditMode.EditMove))
            {
                if (this.Node != null) EditMove_Delta(vector, matrix, inverseMatrix, nodes);
                else RectChoose_Delta(vector, matrix, inverseMatrix, nodes);
            }
        }

        public void Operator_Complete(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix,List<Node> nodes)
        {
            if (this.EditMode == NodeEditMode.Add)
            {
                Add_Complete(vector, matrix, inverseMatrix);
            }
            else if (this.EditMode.HasFlag(NodeEditMode.EditMove))
            {
                if (this.Node != null) EditMove_Complete(vector, matrix, inverseMatrix,nodes);
                else RectChoose_Complete(vector, matrix, inverseMatrix, nodes);
            }
        }


        #endregion

        #region Operator > Add


        private void Add_Start(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            Vector2 inverseMatrixVector = Vector2.Transform(vector, inverseMatrix);

            nodes.Add(new Node(inverseMatrixVector));
        }

        private void Add_Delta(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            Vector2 inverseMatrixVector = Vector2.Transform(vector, inverseMatrix);

            nodes[nodes.Count - 1].SetVector(inverseMatrixVector);
        }

        private void Add_Complete(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix)
        {
        }


        #endregion

        #region Operator > EditMove


        Vector2 EditMoveStart;

        private void EditMove_Start(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            Vector2 inverseMatrixVector = Vector2.Transform(vector, inverseMatrix);

            if (this.Node == null)
            {
                this.RectChooseStart = vector;
                this.RectChooseEnd = vector;
                return;
            }
            switch (this.Node.ChooseMode)
            {
                case NodeChooseMode.None:
                    break;
                case NodeChooseMode.Vector:
                    {
                        EditMoveStart = inverseMatrixVector;
                        if (this.Node.ChooseMode == NodeChooseMode.None)
                        {
                            foreach (var item in nodes)
                            {
                                item.ChooseMode = NodeChooseMode.None;
                            }
                        }
                        this.Node.ChooseMode = NodeChooseMode.Vector;
                        break;
                    }
                case NodeChooseMode.LeftControl:
                    this.Node.SetLeftControlStart(inverseMatrixVector, this.ControlMode);
                    break;
                case NodeChooseMode.RightControl:
                    this.Node.SetRightControlStart(inverseMatrixVector, this.ControlMode);
                    break;
                default: break;
            }
        }

        private void EditMove_Delta(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            Vector2 inverseMatrixVector = Vector2.Transform(vector, inverseMatrix);

            if (this.Node == null)
            {
                this.RectChooseEnd = vector;
                return;
            }

            switch (this.Node.ChooseMode)
            {
                case NodeChooseMode.None:
                    break;
                case NodeChooseMode.Vector:
                    Vector2 offset = inverseMatrixVector - EditMoveStart;

                    foreach (var item in nodes)
                    {
                        if (item.ChooseMode != NodeChooseMode.None)
                        {
                            item.Move(offset);
                        }
                    }

                    this.Node.SetVector(inverseMatrixVector);

                    EditMoveStart = inverseMatrixVector;
                    break;
                case NodeChooseMode.LeftControl:
                    this.Node.SetLeftControlDelta(inverseMatrixVector, this.ControlMode);
                    break;
                case NodeChooseMode.RightControl:
                    this.Node.SetRightControlDelta(inverseMatrixVector, this.ControlMode);
                    break;
                default: break;
            }

        }

        private void EditMove_Complete(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            Vector2 inverseMatrixVector = Vector2.Transform(vector, inverseMatrix);

            if (this.Node == null)
            {
                foreach (var item in nodes)
                {
                    if (item.Vector.X > this.RectChooseStart.X && item.Vector.Y > this.RectChooseStart.Y && item.Vector.X < this.RectChooseEnd.X && item.Vector.Y < this.RectChooseEnd.Y)
                        item.ChooseMode = NodeChooseMode.Vector;
                    else
                        item.ChooseMode = NodeChooseMode.None;
                }

                this.RectChooseStart = this.RectChooseEnd = Vector2.Zero;
                return;
            }

            switch (this.Node.ChooseMode)
            {
                case NodeChooseMode.None:
                    break;
                case NodeChooseMode.Vector:
                    break;
                case NodeChooseMode.LeftControl:
                    this.Node.LeftControl = inverseMatrixVector;
                    break;
                case NodeChooseMode.RightControl:
                    this.Node.RightControl = inverseMatrixVector;
                    break;
                default: break;
            }
        }


        #endregion

        #region Operator > RectChoose


        Vector2 RectChooseStart = new Vector2();
        Vector2 RectChooseEnd = new Vector2();

        private void RectChoose_Start(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix)
        {
            this.EditMode = NodeEditMode.EditMove | NodeEditMode.RectChoose;

            this.RectChooseStart = this.RectChooseEnd = vector;
        }

        private void RectChoose_Delta(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            this.RectChooseEnd = vector;

            //choose point which in rect
            float Left = Math.Min(this.RectChooseStart.X, this.RectChooseEnd.X);
            float Top = Math.Min(this.RectChooseStart.Y, this.RectChooseEnd.Y);
            float Right = Math.Max(this.RectChooseStart.X, this.RectChooseEnd.X);
            float Bottom = Math.Max(this.RectChooseStart.Y, this.RectChooseEnd.Y);
            foreach (var item in nodes)
            {
                if (item.VectorTransform.X > Left && item.VectorTransform.X < Right && item.VectorTransform.Y > Top && item.VectorTransform.Y < Bottom)
                    item.ChooseMode = NodeChooseMode.Vector;
                else
                    item.ChooseMode = NodeChooseMode.None;
            }
        }

        private void RectChoose_Complete(Vector2 vector,Matrix3x2 matrix,Matrix3x2 inverseMatrix, List<Node> nodes)
        {
            this.EditMode = NodeEditMode.EditMove;

            this.RectChoose_Delta(vector, matrix, inverseMatrix,nodes);

            this.RectChooseStart = this.RectChooseEnd = Vector2.Zero;
        }


        #endregion

    }

    /// <summary>  CurveNodes's node </summary>
    public class Node
    {       
        
        //Transform
        public Vector2 VectorTransform { get; private set; }
        public Vector2 LeftControlTransform { get; private set; }
        public Vector2 RightControlTransform { get; private set; }
        public void Transform(Matrix3x2 matrix)
        {
            this.VectorTransform = Vector2.Transform(this.Vector, matrix);
            this.LeftControlTransform = Vector2.Transform(this.LeftControl, matrix);
            this.RightControlTransform = Vector2.Transform(this.RightControl, matrix);
        }


        public Vector2 Vector;
        public Vector2 LeftControl;
        public Vector2 RightControl;

        public bool IsSmooth;
        public NodeChooseMode ChooseMode;

        public Node(Vector2 v, NodeChooseMode mode = NodeChooseMode.None)
        {
            this.Vector = v;
            this.LeftControl = v;
            this.RightControl = v;
            this.ChooseMode = mode;
        }
        public Node(Vector2 v, Vector2 left, Vector2 right, NodeChooseMode mode = NodeChooseMode.None)
        {
            this.Vector = v;
            this.LeftControl = left;
            this.RightControl = right;
            this.ChooseMode = mode;
        }



        /// <summary>
        /// Move nodes
        /// </summary>
        /// <param name="v">the change of position</param>
        public void Move(Vector2 vector)
        {
            this.Vector += vector;
            this.LeftControl += vector;
            this.RightControl += vector;
        }
        /// <summary>
        /// Set the vector of it
        /// </summary>
        /// <param name="v">the position</param>
        public void SetVector(Vector2 vector)
        {
            Vector2 changed = vector - this.Vector;

            this.Vector = vector;
            this.LeftControl += changed;
            this.RightControl += changed;
        }




        private float CacheLeftControlDistance;
        private float CacheRightControlDistance;

        public void SetLeftControlStart(Vector2 v, NodeControlMode controlMode)
        {
            if (controlMode == NodeControlMode.Asymmetric)
            {
                this.CacheRightControlDistance = Vector2.Distance(this.Vector, this.RightControl);
            }

            this.SetLeftControlDelta(v, controlMode);
        }
        public void SetRightControlStart(Vector2 v, NodeControlMode controlMode)
        {
            if (controlMode == NodeControlMode.Asymmetric)
            {
                this.CacheLeftControlDistance = Vector2.Distance(this.Vector, this.LeftControl);
            }

            this.SetRightControlDelta(v, controlMode);
        }

        public void SetLeftControlDelta(Vector2 v, NodeControlMode controlMode)
        {
            this.LeftControl = v;

            switch (controlMode)
            {
                case NodeControlMode.Mirrored:
                    this.RightControl = 2 * this.Vector - v;
                    break;
                case NodeControlMode.Disconnected:
                    break;
                case NodeControlMode.Asymmetric:
                    float distance = Vector2.Distance(this.Vector, v);
                    float sin = (this.Vector.Y - v.Y) / distance;
                    float cos = (this.Vector.X - v.X) / distance;
                    this.RightControl.X = this.Vector.X + cos * this.CacheRightControlDistance;
                    this.RightControl.Y = this.Vector.Y + sin * this.CacheRightControlDistance;
                    break;
                default:
                    break;
            }
        }
        public void SetRightControlDelta(Vector2 v, NodeControlMode controlMode)
        {
            this.RightControl = v;

            switch (controlMode)
            {
                case NodeControlMode.Mirrored:
                    this.LeftControl = 2 * this.Vector - v;
                    break;
                case NodeControlMode.Disconnected:
                    break;
                case NodeControlMode.Asymmetric:
                    float distance = Vector2.Distance(this.Vector, this.RightControl);
                    float sin = (this.Vector.Y - this.RightControl.Y) / distance;
                    float cos = (this.Vector.X - this.RightControl.X) / distance;
                    this.LeftControl.X = this.Vector.X + cos * this.CacheLeftControlDistance;
                    this.LeftControl.Y = this.Vector.Y + sin * this.CacheLeftControlDistance;
                    break;
                default:
                    break;
            }
        }




        /// <summary>
        /// Returns true if a vector or a control point is in the dot
        /// 如果点中了向量或某个控制点，返回true
        /// </summary>
        /// <param name="v">the position</param>
        /// <returns></returns>
        public NodeChooseMode GetChooseMode(Vector2 vector)
        {
            if (Vector2.DistanceSquared(vector, this.VectorTransform) < 100) return NodeChooseMode.Vector; ;

            if (this.ChooseMode != NodeChooseMode.None)
            {
                if (Vector2.DistanceSquared(vector, this.LeftControlTransform) < 100) return NodeChooseMode.LeftControl;
                else if (Vector2.DistanceSquared(vector, this.RightControlTransform) < 100) return NodeChooseMode.RightControl;
            }

            return NodeChooseMode.None;
        }



    }




    /// <summary>  CurveNodes's edit mode </summary>
    [Flags]
    public enum NodeEditMode
    {
        /// <summary>  add new node </summary>
        Add = 1,

        /// <summary> edit the nodes </summary>
        EditMove = 2,

        /// <summary> choose nodes in the blue rect </summary>
        RectChoose = 4,
    }

    /// <summary> vector or control point </summary>
    public enum NodeChooseMode
    {
        /// <summary> none </summary>
        None,

        /// <summary> main point </summary>
        Vector,

        /// <summary> left control point </summary>
        LeftControl,

        /// <summary>  right control point </summary>
        RightControl,
    }

    /// <summary> control mode of the control point  </summary>
    public enum NodeControlMode
    {
        /// <summary> Two control point mirroring </summary>
        Mirrored,

        /// <summary> Two control points disconnected  </summary>
        Disconnected,

        /// <summary> Two points in a straight line </summary>
        Asymmetric,
    }
    
}