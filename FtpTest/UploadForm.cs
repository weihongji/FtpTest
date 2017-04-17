using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpTest
{
	public partial class UploadForm : Form
	{
		public UploadForm() {
			InitializeComponent();
		}

		private void UploadForm_Load(object sender, EventArgs e) {
			// US: butlerrecreation
			//this.txtHost.Text = "sftp://159.242.130.2/export/home/activenet";
			this.txtHost.Text = "sftp://159.242.130.2//export/home/activenet";
			this.txtPort.Text = "22";
			this.txtUser.Text = "activnet";
			this.txtPassword.Text = @"tier-98Z4s%8!55vf";

			// CA: UofGconnect
			//this.txtHost.Text = "sftp://easftp2.ccs.uoguelph.ca/data/file_exports";
			//this.txtPort.Text = "22";
			//this.txtUser.Text = "activenet";
			//this.txtPassword.Text = @"h6*7oat\%S";
		}

		private void btnOpen_Click(object sender, EventArgs e) {
			if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				this.txtFile.Text = this.openFileDialog1.FileName;
			}
			else {
				this.txtFile.Text = "";
			}
		}

		private void btnUpload_Click(object sender, EventArgs e) {
			if (this.txtFile.Text.Length == 0) {
				MessageBox.Show("No file selected to upload.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			int port = 0;
			if (this.txtPort.Text.Length > 0 && !int.TryParse(this.txtPort.Text, out port)) {
				MessageBox.Show("Invalid port value", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				this.txtPort.Focus();
				return;
			}

			var cursor = this.Cursor;
			try {
				this.Cursor = Cursors.WaitCursor;
				FtpUltility.FTPUpload(this.txtFile.Text, this.txtHost.Text, port, this.txtUser.Text, this.txtPassword.Text);
				MessageBox.Show("Uploaded", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex) {
				this.Cursor = cursor;
				string msg = string.Format("{0}\r\n\r\n{1}", ex.Message, "More details?");
				if (MessageBox.Show(msg, this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.OK) {
					msg = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
					MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
				return;
			}
			this.Cursor = cursor;
		}
	}
}
