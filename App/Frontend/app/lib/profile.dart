import 'package:app/Notification.dart';
import 'package:app/RemovePost/body.dart';
import 'package:app/favorite/FavoritePage.dart';
import 'package:flutter/material.dart';
import 'editprofile.dart';
import 'homeuser.dart';
import 'Login.dart';

/**************Global variables******************/
double scw = 0;
double sch = 0;
double t = 0;
double wid(double w) {
  return scw * (w / 380);
}

double hig(double h) {
  return (h / 592) * sch;
}

class ProfilePage extends StatelessWidget {
  @override
  ProfilePage({super.key});
  Widget build(BuildContext context) {
    return  WillPopScope(
      onWillPop: () async {
        return false;
      },
      child: Scaffold(
        body: ProfileBody(),
        bottomNavigationBar: BottomNavBar(),
      ),
    );
  }
}

/*********************main widget*************************/
class ProfileBody extends StatefulWidget {
  @override
  State<ProfileBody> createState() => _ProfileBodyState();
}

class _ProfileBodyState extends State<ProfileBody> {
  bool profileLoading = false;
  bool NoError = true;

  /******************message show when click on delete**********************/
  void showAlertDialog(BuildContext context) {
    /*******Cancel button***********/
    Widget cancelButton = TextButton(
      child: Text("Cancel"),
      onPressed: () {
        Navigator.of(context, rootNavigator: true)
            .pop(); 
      },
    );
    /*******Continue button***********/
    Widget continueButton = TextButton(
      child: Text("Continue"),
      onPressed: () async {
        Navigator.of(context, rootNavigator: true)
            .pop(); 
        setState(() {
          profileLoading = true;
        });
        /**********api for delete account***************/
        NoError = await user.delete();
        setState(() {
          profileLoading = false;
        });
        if(NoError)
        {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => Login()),
        );   
        }
      },
    );

    /************set up the AlertDialog**************/
    AlertDialog alert = AlertDialog(
      title: Text("Warning Message"),
      content: Text("Are you sure you want to delete your account?"),
      actions: [
        cancelButton,
        continueButton,
      ],
    );

    /************show the dialog***************/
    showDialog(
      context: context,
      builder: (BuildContext dialogContext) {
        return alert;
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width;
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;

    return Center(
      child: Stack(
        children: [
          Column(
            crossAxisAlignment: CrossAxisAlignment.center,
            children: <Widget>[
              /***************header************/
              ProfileHeader(
                wid: wid,
                hig: hig,
                t: t,
              ),
              SizedBox(height: hig(20)),
              /***********if api return error***********/
              if (!NoError)
                Center(
                  child: Text(
                    "Oops! Something went wrong.",
                    style: TextStyle(fontSize: 15.0, color: Colors.red),
                  ),
                ),
              /*****************edie profile***************/
              ProfileMenuItem(
                text: 'Edit Profile',
                onTap: () {
                  Navigator.pushReplacement(
                    context,
                    MaterialPageRoute(builder: (context) => EditProfile()),
                  );
                },
                wid: wid,
                t: t,
              ),
              buildDivider(wid, hig),
              /*****************Favorites***************/
              ProfileMenuItem(
                text: 'Favorites',
                onTap: () {
                  Navigator.pushReplacement(
                    context,
                    MaterialPageRoute(builder: (context) => FavoritePage()),
                  );

                  print('Favorites clicked');
                },
                wid: wid,
                t: t,
              ),
              buildDivider(wid, hig),
              /*****************your posts***************/
              ProfileMenuItem(
                text: 'Your Posts',
                onTap: () {
                  Navigator.pushReplacement(
                    context,
                    MaterialPageRoute(builder: (context) => RemoveBody()),
                  );

                  print('Your Posts clicked');
                },
                wid: wid,
                t: t,
              ),
              buildDivider(wid, hig),
              /*****************Delete account***************/
              ProfileMenuItem(
                text: 'Delete Account',
                onTap: () {
                  showAlertDialog(context);
                },
                wid: wid,
                t: t,
              ),
              buildDivider(wid, hig),
              /*****************Log out***************/
              ProfileMenuItem(
                text: 'Log out',
                onTap: () {
                  user.username ='';
                  Navigator.pushReplacement(
                    context,
                    MaterialPageRoute(builder: (context) => Login()),
                  );

                  print('Log out clicked');
                },
                wid: wid,
                t: t,
              ),
              Spacer(),
            ],
          ),
          if (profileLoading)
            const Opacity(
                opacity: 0.8,
                child: ModalBarrier(dismissible: false, color: Colors.black)),
          if (profileLoading)
            const Center(
              child: CircularProgressIndicator(),
            )
        ],
      ),
    );
  }

/***************widget for create grey divider*****************/
  Widget buildDivider(Function(double) wid, Function(double) hig) {
    return Container(
      height: hig(3),
      width: wid(273.35),
      color: Color(0xffD1D1D1),
    );
  }
}

/*****************widget for create user picture and his name and email and image*****************/
class ProfileHeader extends StatelessWidget {
  final Function(double) wid;
  final Function(double) hig;
  final double t;

  const ProfileHeader({
    required this.wid,
    required this.hig,
    required this.t,
  });

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: double.infinity,
      height: hig(220),
      child: Stack(
        alignment: Alignment.center,
        children: [
          /**********user image************/
          Positioned(
            top: hig(30),
            child: ClipOval(
              child: Image.network(
                user.image,
                width: hig(80),
                height: hig(80),
                fit: BoxFit.fill,
              ),
            ),
          ),
          /*************user name *************/
          Positioned(
            top: hig(110),
            child: Text(
              user.username,
              style: TextStyle(
                color: Color(0xff063221),
                fontSize: 17 * t,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
          /************user email************/
          Positioned(
            top: hig(130),
            child: Text(
              user.email,
              style: TextStyle(
                color: Color(0xff607C71),
                fontSize: 12 * t,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
          /***********green image************/
          Positioned(
            top: hig(100),
            left: 0,
            right: 0,
            child: Image.asset(
              'assets/profile/9.png',
              width: double.infinity,
              height: hig(120),
              fit: BoxFit.fill,
            ),
          ),
        ],
      ),
    );
  }
}

/****************widget for create diffrent choices*******************/
class ProfileMenuItem extends StatelessWidget {
  final String text;
  final VoidCallback onTap;
  final Function(double) wid;
  final double t;

  const ProfileMenuItem({
    required this.text,
    required this.onTap,
    required this.wid,
    required this.t,
  });

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 10),
        child: Row(
          children: [
            SizedBox(width: wid(37.5)),
            Text(
              text,
              style: TextStyle(
                color: Color(0xff607C71),
                fontSize: 15 * t,
                fontWeight: FontWeight.bold,
              ),
            ),
          ],
        ),
      ),
    );
  }
}

/*********************widget for bottom navigation bar*************************/
class BottomNavBar extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
        color: Color(0xffECECEC),
        borderRadius: BorderRadius.vertical(
          top: Radius.circular(20),
        ),
      ),
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 25),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            /************Message***********/
            _buildIconItem(Icons.message, 'Messages', () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => Messages()),
              );
            }),
            /************home***********/
            _buildIconItem(Icons.home, 'Home', () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(
                    builder: (context) => HomeUser(
                          userr: user,
                        )),
              );
            }),
            /************profile***********/
            _buildIconItem(Icons.person, 'Profile', () {
              // Add onTap functionality for Profile
            }),
          ],
        ),
      ),
    );
  }

/***************widget for each item in navigation bar****************/
  Widget _buildIconItem(IconData iconData, String text, VoidCallback onTap) {
    return GestureDetector(
      onTap: onTap,
      child: SizedBox(
        height: hig(62),
        child: Center(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(
                iconData,
                size: 30,
                color: text == "Profile"
                    ? Color.fromARGB(136, 6, 50, 33)
                    : Color(0xff063221),
              ),
              SizedBox(height: 3),
              Text(
                text,
                style: TextStyle(
                  fontSize: 10,
                  fontWeight: FontWeight.bold,
                  color: Color(0xff063221),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
