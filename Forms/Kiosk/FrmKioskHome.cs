using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CinemaManagement.Data;
using CinemaManagement.Models;

namespace CinemaManagement.Forms.Kiosk;

public class FrmKioskHome : Form

{
    private FlowLayoutPanel pnlMovieList;
 private Label  lblHeader;

 public FrmKioskHome()
 {
     InitializeComponent();
     LoadMovies();
 }
 private void   InitializeComponent()
 {
     this.Text = "CineLuxe Kiosk - Trang chủ";
     this.Size =  new Size(1200, 500);
     this.StartPosition = FormStartPosition.CenterScreen;
     this.BackColor  = Color.FromArgb(18,24,38);
     lblHeader = new Label()
     {
         Text = "Đang chiếu tại rạp",
             Font = new Font("Segoe UI",16F,FontStyle.Bold),
         ForeColor  = Color.White,
         Dock = DockStyle.Top,
         Height = 60,
         TextAlign = ContentAlignment.MiddleCenter
     };
     pnlMovieList = new FlowLayoutPanel
     {
         Dock = DockStyle.Fill,
         AutoScroll =  true,
         Padding = new Padding(15)
     };
     Controls.Add(pnlMovieList);
     Controls.Add(lblHeader);
 }
 private void  LoadMovies() 
 {
     pnlMovieList.Controls.Clear();
     using var db = new CinemaDbContext();
     var movies = db.Movies.ToList();
     foreach (var movie in movies)
     {
         Panel card =  new Panel
         {
             Width = 440,
             Height = 130,
             BackColor = Color.FromArgb(30,41,59),
             Margin = new Padding(0,0,0,15),
         };
         Label lblTitle = new Label
         {
             Text = movie.Title.ToUpper(),
             Font = new Font("Segoe UI", 12F,FontStyle.Bold),
             ForeColor =  Color.Gold,
             Location = new Point(15,15),
             AutoSize =  true
         };
         Label lblGenre = new Label
         {
             Text = $"Thể loại: {movie.Genre} | Thời lượng : {movie.Duration} phút ",
             Font = new Font("Segoe UI",9F),
             ForeColor  =  Color.LightGray,
             Location = new Point(15,45),
             AutoSize =  true
         };
         Button btnBook = new Button
         {
             Text = " Đặt vé ngay",
             Font = new Font("Segoe UI", 10F,FontStyle.Bold),
             BackColor = Color.FromArgb(180,100,0),
             ForeColor =   Color.White,
             FlatStyle = FlatStyle.Flat,
             Size  = new Size(140,38),
             Location = new Point(285,75),
             Cursor = Cursors.Hand
         };
         btnBook.Click +=(s,e)=>
             {
                 FrmKioskShowtime frmShowtime = new FrmKioskShowtime(movie.MovieId);
                 frmShowtime.Show();
                 this.Hide();
                 
             ;
        card.Controls.Add(lblTitle);
        card.Controls.Add(lblGenre);
        card.Controls.Add(btnBook);
 pnlMovieList.Controls.Add(card);
 }; 
 }
 }
 }