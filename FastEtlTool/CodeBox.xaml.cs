using FastEtlTool.Base;
using System.Windows;

namespace FastEtlTool
{
    /// <summary>
    /// CodeBox.xaml 的交互逻辑
    /// </summary>
    public partial class CodeBox : Window
    {
        public CodeBox()
        {
            InitializeComponent();
            Common.InitWindows(this, "消息", false);
        }

        /// <summary>
        /// 内容消息
        /// </summary>
        public string Message
        {
            get { return this.labMessage.Text.ToString(); }
            set { this.labMessage.Text = value; }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static void Show(string msg, Window owner)
        {
            var msgBox = new CodeBox();
            msgBox.Message = msg;
            Common.OpenWin(msgBox, owner);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
