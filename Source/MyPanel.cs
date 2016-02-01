using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PDFScanningApp
{
  class MyPanel : Panel
  {
    public MyPanel(Image thumbnail)
    {
      MyPictureBox myPictureBox = new MyPictureBox();
      myPictureBox.Image = thumbnail;
      myPictureBox.Dock = DockStyle.Fill;
      myPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
      this.Controls.Add(myPictureBox);
      AllowDrag = true;
    }

    public bool AllowDrag { get; set; }
    private bool _isDragging = false;
    private int _DDradius = 2;

    private int _mX = 0;
    private int _mY = 0;

    protected override void OnGotFocus(EventArgs e)
    {
      this.BackColor = Color.SandyBrown;
      base.OnGotFocus(e);
    }

    protected override void OnLostFocus(EventArgs e)
    {
      this.BackColor = Color.Red;
      base.OnLostFocus(e);
    }

    protected override void OnClick(EventArgs e)
    {
      this.Focus();
      base.OnClick(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      this.Focus();
      base.OnMouseDown(e);
      _mX = e.X;
      _mY = e.Y;
      this._isDragging = false;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      if (!_isDragging)
      {
        // This is a check to see if the mouse is moving while pressed.
        // Without this, the DragDrop is fired directly when the control is clicked, now you have to drag a few pixels first.
        if (e.Button == MouseButtons.Left && _DDradius > 0 && this.AllowDrag)
        {
          int num1 = _mX - e.X;
          int num2 = _mY - e.Y;
          if (((num1 * num1) + (num2 * num2)) > _DDradius)
          {
            DoDragDrop(this, DragDropEffects.All);
            _isDragging = true;
            return;
          }
        }
        base.OnMouseMove(e);
      }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      _isDragging = false;
      base.OnMouseUp(e);
    }   

  }
}
