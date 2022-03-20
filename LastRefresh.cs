#r "System.Drawing"

using System.Drawing;


// Parameters
int formWidth = 600;
int formHeight = 700;
string ebiURL = @"https://dennestorres.com";
int gap = 10;

//Fonts
System.Drawing.Font elegantFont = new Font("Century Gothic", 10, FontStyle.Italic);

//Colors
System.Drawing.Color bkgrdColor =  ColorTranslator.FromHtml("#F2F2F2");


// Start screen
System.Windows.Forms.Form newForm = new System.Windows.Forms.Form();
System.Windows.Forms.ImageList imageList = new System.Windows.Forms.ImageList();
System.Windows.Forms.ImageList imageList2 = new System.Windows.Forms.ImageList();
System.Net.WebClient w = new System.Net.WebClient();

//Form
newForm.TopLevel = true;
newForm.BackColor = bkgrdColor;
newForm.Text = "Last Refresh";
newForm.Size = new Size(formWidth,formHeight);
newForm.MaximumSize = new Size(formWidth,formHeight);
newForm.MinimumSize = new Size(formWidth,formHeight);


// Main screen
System.Windows.Forms.TreeView treeView = new System.Windows.Forms.TreeView();
System.Windows.Forms.LinkLabel ebiHome = new System.Windows.Forms.LinkLabel();

//Designed by
newForm.Controls.Add(ebiHome);
ebiHome.Text = "Designed by DTower Software inspired by Elegant BI";
ebiHome.Size = new Size(400,40);
ebiHome.Location = new Point(50,560);
ebiHome.Font = elegantFont;

ebiHome.LinkClicked += (System.Object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e) => {

    System.Diagnostics.Process.Start(ebiURL);
};

newForm.Controls.Add(treeView);
treeView.Visible = true;
treeView.Size = new Size(formWidth - 35,480);
treeView.Location = new Point(gap,gap);
treeView.StateImageList = new System.Windows.Forms.ImageList();
treeView.BackColor = Color.White;
treeView.CheckBoxes = false;

// Add images from web to Image List
var urlPrefix = "https://raw.githubusercontent.com/m-kovalsky/Tabular/master/Icons/";
var urlSuffix = "Icon.png";

string[] imageURLList = { "Table", "Partition", "SummaryTable", "SummaryPartition", "Model", "SummaryModel","SaveDark", "Script", "Delete","ForwardArrow", "BackArrow"};
for (int i = 0; i < imageURLList.Count(); i++)
{
    var url = urlPrefix + imageURLList[i] + urlSuffix;      
    byte[] imageByte = w.DownloadData(url);
    System.IO.MemoryStream ms = new System.IO.MemoryStream(imageByte);
    System.Drawing.Image im = System.Drawing.Image.FromStream(ms);

    if (i<6)
    {
        imageList.Images.Add(im);
    }
    else
    {
        imageList2.Images.Add(im);
    }
}  


//Fill the treeview
imageList.ImageSize = new Size(16, 16); 
treeView.ImageList = imageList;
        treeView.Nodes.Clear();
        // Add model node
        string modelName = Model.Database.Name;
        var mn = treeView.Nodes.Add(modelName);
        mn.ImageIndex = 4;
        mn.SelectedImageIndex = 4;
        mn.StateImageIndex = 0;

foreach (var t in Model.Tables.OrderBy(a => a.Name).ToList())
        {  
            // Add table nodes
            string tableName = t.Name;    
            var tn = mn.Nodes.Add(tableName);
            tn.ImageIndex = 0;
            tn.SelectedImageIndex = 0;
            tn.StateImageIndex = 0;    

	    var maxRefreshedTime=DateTime.MinValue;
            // Add partition sub-nodes
            foreach (var p in t.Partitions.OrderBy(a => a.Name).ToList())
            {
                string pName = p.Name;
                var x = tn.Nodes.Add(pName + "                       " + p.RefreshedTime.ToString());  

                x.ImageIndex = 1;
                x.SelectedImageIndex = 1;
                x.StateImageIndex = 0;
		if (p.RefreshedTime > maxRefreshedTime)
			maxRefreshedTime=p.RefreshedTime;
            }
	    tn.Text += "            " + maxRefreshedTime.ToString();
        }

newForm.Show();