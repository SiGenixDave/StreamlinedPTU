#region --- Revision History ---
/*
 * 
 *  This document and its contents are the property of Bombardier Inc. or its subsidiaries and contains confidential, proprietary information.
 *  The reproduction, distribution, utilization or the communication of this document, or any part thereof, without express authorization is strictly prohibited.  
 *  Offenders will be held liable for the payment of damages.
 * 
 *  (C) 2010    Bombardier Inc. or its subsidiaries. All rights reserved.
 * 
 *  Solution:   Portable Test Unit
 * 
 *  Project:    Common
 * 
 *  File name:  FormDataStreamPlot.Designer.cs
 * 
 *  Revision History
 *  ----------------
 * 
 *  Date        Version Author          Comments
 *  11/09/10    1.0     K.McD           1   Initial entry into TortoiseSVN.
 *  
 *  08/10/11    1.14    K.McD           1.  Changed the BackColor property of one or more controls to Transparent.
 *  
 *  05/19/18    1.20    Vgott           1. Updated Icon as ScreenShot for F2 function key.
 */
#endregion --- Revision History ---

using Common.UserControls;

namespace Common.Forms
{
    partial class FormDataStreamPlot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataStreamPlot));
            this.m_PanelSupplementalInformation = new System.Windows.Forms.Panel();
            this.m_LabelSupplementalInformation = new System.Windows.Forms.Label();
            this.m_LegendSupplementalInformation = new System.Windows.Forms.Label();
            this.m_printPreviewDialogPTU = new System.Windows.Forms.PrintPreviewDialog();
            this.m_TabControl.SuspendLayout();
            this.m_PanelInformation.SuspendLayout();
            this.m_PanelSupplementalInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_TabControl
            // 
            this.m_TabControl.Location = new System.Drawing.Point(0, 90);
            this.m_TabControl.Size = new System.Drawing.Size(1200, 610);
            // 
            // m_PanelInformation
            // 
            this.m_PanelInformation.Controls.Add(this.m_PanelSupplementalInformation);
            this.m_PanelInformation.Location = new System.Drawing.Point(0, 57);
            this.m_PanelInformation.Controls.SetChildIndex(this.m_PanelSupplementalInformation, 0);
            // 
            // m_PanelSupplementalInformation
            // 
            this.m_PanelSupplementalInformation.AutoSize = true;
            this.m_PanelSupplementalInformation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.m_PanelSupplementalInformation.BackColor = System.Drawing.Color.Transparent;
            this.m_PanelSupplementalInformation.Controls.Add(this.m_LabelSupplementalInformation);
            this.m_PanelSupplementalInformation.Controls.Add(this.m_LegendSupplementalInformation);
            this.m_PanelSupplementalInformation.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_PanelSupplementalInformation.Location = new System.Drawing.Point(828, 0);
            this.m_PanelSupplementalInformation.Name = "m_PanelSupplementalInformation";
            this.m_PanelSupplementalInformation.Size = new System.Drawing.Size(370, 31);
            this.m_PanelSupplementalInformation.TabIndex = 0;
            this.m_PanelSupplementalInformation.Visible = false;
            // 
            // m_LabelSupplementalInformation
            // 
            this.m_LabelSupplementalInformation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.m_LabelSupplementalInformation.AutoEllipsis = true;
            this.m_LabelSupplementalInformation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_LabelSupplementalInformation.Location = new System.Drawing.Point(130, 4);
            this.m_LabelSupplementalInformation.Name = "m_LabelSupplementalInformation";
            this.m_LabelSupplementalInformation.Size = new System.Drawing.Size(230, 23);
            this.m_LabelSupplementalInformation.TabIndex = 0;
            this.m_LabelSupplementalInformation.Text = "<Supplemental>";
            this.m_LabelSupplementalInformation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_LegendSupplementalInformation
            // 
            this.m_LegendSupplementalInformation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_LegendSupplementalInformation.AutoSize = true;
            this.m_LegendSupplementalInformation.Location = new System.Drawing.Point(0, 8);
            this.m_LegendSupplementalInformation.Name = "m_LegendSupplementalInformation";
            this.m_LegendSupplementalInformation.Size = new System.Drawing.Size(55, 13);
            this.m_LegendSupplementalInformation.TabIndex = 0;
            this.m_LegendSupplementalInformation.Text = "<Legend>";
            this.m_LegendSupplementalInformation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_printPreviewDialogPTU
            // 
            this.m_printPreviewDialogPTU.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.m_printPreviewDialogPTU.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.m_printPreviewDialogPTU.ClientSize = new System.Drawing.Size(400, 300);
            this.m_printPreviewDialogPTU.Enabled = true;
            this.m_printPreviewDialogPTU.Icon = ((System.Drawing.Icon)(resources.GetObject("m_printPreviewDialogPTU.Icon")));
            this.m_printPreviewDialogPTU.Name = "m_printPreviewDialogPTU";
            this.m_printPreviewDialogPTU.ShowIcon = false;
            this.m_printPreviewDialogPTU.Visible = false;
            // 
            // FormDataStreamPlot
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Name = "FormDataStreamPlot";
            this.Text = "Plot Data Stream";
            this.Shown += new System.EventHandler(this.FormDataStreamPlot_Shown);
            this.m_TabControl.ResumeLayout(false);
            this.m_PanelInformation.ResumeLayout(false);
            this.m_PanelInformation.PerformLayout();
            this.m_PanelSupplementalInformation.ResumeLayout(false);
            this.m_PanelSupplementalInformation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Reference to the <c>Panel</c> control used to display the supplemental information associated with the data stream.
        /// </summary>
        protected System.Windows.Forms.Panel m_PanelSupplementalInformation;

        /// <summary>
        /// Reference to the <c>Label</c> control that is used to display the supplemental text.
        /// </summary>
        protected System.Windows.Forms.Label m_LabelSupplementalInformation;

        /// <summary>
        /// Reference to the <c>Label</c> control used to display the legend associated with the supplemental information.
        /// </summary>
        protected System.Windows.Forms.Label m_LegendSupplementalInformation;
        private System.Windows.Forms.PrintPreviewDialog m_printPreviewDialogPTU;
    }
}