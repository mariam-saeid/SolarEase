import 'dart:async';
import 'package:app/inputCalculation.dart';
import 'package:app/result.dart';
import 'package:app/MarketPlace/MarketPlace.dart';
import 'package:app/NoSystemSize.dart';
import 'package:app/Notification.dart';
import 'package:app/api_objects/resultsclasses.dart';
import 'package:app/api_objects/user.dart';
import 'package:flutter/material.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'Chatbot.dart';
import 'Prediction.dart';
import 'findSolarInstallers.dart';
import 'profile.dart';
import 'tpsAndGuides.dart';

/*********************Global variables********************/
bool homeloading = false;
bool isexist = false;
double scw = 0;
double sch = 0;
System system = System(inpudata: inpudata);
User user = User(
  username: "username",
  password: "password",
  email: "email",
  location: "location",
  phoneNumber: "phoneNumber",
  city: "city",
);
double wid(double w) => scw * (w / 415);
double hig(double h) => (h / 592) * sch;

////////////////////////////////////////////
class HomeUser extends StatefulWidget {
  HomeUser({super.key, required userr}) {
    user = userr;
  }
  @override
  _HomeUserState createState() => _HomeUserState();
}

/************************main widget*************************************/
class _HomeUserState extends State<HomeUser> {
  late PageController _pageController; // Controller for the PageView
  int _currentPage = 0; // Tracks the current page index
  Timer? _autoScrollTimer;

  @override
  void initState() {
    super.initState();
    _pageController = PageController(initialPage: 0, viewportFraction: 0.60);
    _startAutoScroll(); // Start auto-scrolling the PageView
  }

  @override
  void dispose() {
    _pageController.dispose(); // Dispose the PageController
    _autoScrollTimer?.cancel(); // Cancel the auto-scroll timer
    super.dispose();
  }

  /****************Function to start auto-scrolling the PageView**************/
  void _startAutoScroll() {
    _autoScrollTimer = Timer.periodic(Duration(seconds: 2), (timer) {
      if (!mounted) return; // Check if the widget is still mounted
      setState(() {
        int nextPage = (_currentPage + 1) % 5;
        _pageController.animateToPage(
          nextPage,
          duration: Duration(milliseconds: 500),
          curve: Curves.ease,
        );
        _currentPage = nextPage;
      });
    });
  }
  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width;
    sch = MediaQuery.of(context).size.height;
    return ScreenUtilInit(
        designSize: Size(scw, sch),
        builder: (context, child) {
          return  WillPopScope(
            onWillPop: () async {
              // Return false to disable the back button
              return false;
            },
            child: Scaffold(
              body: Stack(
                children: [
                  Column(
                    children: [
                      /*********************Header ***********************/
                      _buildHeader(context, hig, wid),
                      SizedBox(height: hig(29.6)),
                      /*************Menu with different options***********/
                      _buildMenu(context, hig, wid, sch, scw),
                      SizedBox(height: hig(23.68)),
                      /*********Section for "Your Path For Progress********/
                      _buildPathForProgress(context, hig, wid),
                      SizedBox(height: hig(17.76)),
                      /**********PageView for displaying images************/
                      _buildPageView(context, hig, wid),
                      SizedBox(height: hig(17.76)),
                      /******************Page indicator********************/
                      PageIndicator(currentPage: _currentPage, pageCount: 5),
                    ],
                  ),
                  if (homeloading)
                    const Opacity(
                        opacity: 0.8,
                        child: ModalBarrier(
                            dismissible: false, color: Colors.black)),
                  if (homeloading)
                    const Center(
                      child: CircularProgressIndicator(),
                    )
                ],
              ),
              // Bottom navigation bar
              bottomNavigationBar: BottomNavBar(),
              // ),
            ),
          );
        });
  }

/*****************Widget to build the header section**********************/
  Widget _buildHeader(BuildContext context, double Function(double) hig,
      double Function(double) wid) {
    return SizedBox(
      height: hig(124.32),
      child: Stack(
        children: [
          Image.asset(
            'assets/homeuser/3.png',
            fit: BoxFit.fill,
            height: hig(112.48),
            width: double.infinity,
          ),
          Positioned(
            top: hig(41.44),
            left: 0,
            right: 0,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  'Solar',
                  style: TextStyle(
                    fontSize: 30.0.sp,
                    fontWeight: FontWeight.bold,
                    color: Color(0xff063221),
                  ),
                ),
                Text(
                  'Ease',
                  style: TextStyle(
                    fontSize: 30.0.sp,
                    fontWeight: FontWeight.bold,
                    color: Color(0xffFB9534),
                  ),
                ),
              ],
            ),
          ),
          Positioned(
            // top: (hig(112.48)) - hig(22),
            bottom: 0,
            left: wid(41.5),
            child: CircleAvatar(
              backgroundImage: NetworkImage(user.image),
              radius: 21.0,
            ),
          ),
          Positioned(
            // top: hig(112.48) - hig(11),
            bottom: 10.5,
            left: wid(103.75),
            child: Text(
              'Hi ' + user.username + ' !',
              style: TextStyle(
                fontSize: 17.0.sp,
                fontWeight: FontWeight.bold,
                color: Color(0xff063221),
              ),
            ),
          ),
        ],
      ),
    );
  }

/******************Widget to build the menu section with different options********************/
  Widget _buildMenu(BuildContext context, double Function(double) hig,
      double Function(double) wid, double sch, double scw) {
    return Container(
      decoration: BoxDecoration(
        color: Color(0xffDDEFD0),
        borderRadius: BorderRadius.circular(20.0),
      ),
      child: SizedBox(
        height: hig(148),
        width: wid(402.55),
        child: Column(
          children: [
            Spacer(flex: 1),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                menueitem(Icons.calculate, 'Calculations', sch, scw, context,
                    InputCalculations()),
                menueitem(Icons.shopping_cart, 'Marketplace', sch, scw, context,
                    MarketPlace()),
                menueitem(Icons.trending_up, 'Prediction', sch, scw, context,
                    Prediction()),
              ],
            ),
            Spacer(flex: 1),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceEvenly,
              children: [
                menueitem(Icons.location_on, 'Solar Installers', sch, scw,
                    context, FindSolarInstallers()),
                menueitem(Icons.chat, 'Chatbot', sch, scw, context, Chatbot()),
                menueitem(Icons.lightbulb_outline, 'About Us', sch, scw, context,
                    TipsAndGuides()),
              ],
            ),
            Spacer(flex: 1),
          ],
        ),
      ),
    );
  }

/*****************Widget to build the "Your Path For Progress" section*************************/
  Widget _buildPathForProgress(BuildContext context,
      double Function(double) hig, double Function(double) wid) {
    return Row(
      children: [
        SizedBox(width: 10),
        Container(
          decoration: BoxDecoration(
            color: Color(0xffFB9534),
            borderRadius: BorderRadius.circular(20.0),
          ),
          width: wid(12.45),
          height: hig(35.52),
        ),
        SizedBox(width: wid(12.45)),
        Text(
          'Your Path For Progress',
          style: TextStyle(
            fontSize: 17.0.sp,
            fontWeight: FontWeight.bold,
            color: Color(0xff063221),
          ),
        ),
      ],
    );
  }

/***************Widget to build the PageView for displaying images**************/
  Widget _buildPageView(BuildContext context, double Function(double) hig,
      double Function(double) wid) {
    return SizedBox(
      height: hig(118.4),
      child: PageView.builder(
        padEnds: false,
        controller: _pageController,
        itemCount: 5,
        onPageChanged: (int page) {
          setState(() {
            _currentPage = page;
          });
        },
        itemBuilder: (BuildContext context, int index) {
          double viewportFraction = index == 0 ? 0.6 : 0.2;
          EdgeInsets padding = EdgeInsets.symmetric(horizontal: wid(8.3));

          return SizedBox(
            width: MediaQuery.of(context).size.width * viewportFraction,
            child: Padding(
              padding: padding,
              child: ClipRRect(
                borderRadius: BorderRadius.circular(10.0),
                child: Image.asset(
                  'assets/homeuser/${index + 4}.png',
                  fit: BoxFit.fill,
                ),
              ),
            ),
          );
        },
      ),
    );
  }

/********************** Widget to build an individual icon item of menu****************************/
  Widget menueitem(IconData iconData, String text, double sch, double scw,
      BuildContext context, Widget? route) {
    return GestureDetector(
      onTap: () async {
        /****************if prediction*****************/
        if (text == "Prediction") {
          /********Not have system size********/
          if (user.systemsize == "0") {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) =>NoSystemSize()),
            );

          }
          /******** have system size********/
          else {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) =>route!),
            );
          }
        }
        /*************not caculations****************/
        else if (text != "Calculations") {
          if (route != null) {
            Navigator.pushReplacement(
              context,
              MaterialPageRoute(builder: (context) => route!),
            );
          }
        }
        /*************Calculation**************/
        else {
          if (route != null) {
            setState(() {
              homeloading = true;
            });
            int res = await system.isExist();
            setState(() {
              homeloading = false;
            });
            /************if exist******************/
            if (res == 200) {
              isexist = true;
               Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) =>Results() ),
              );
           
            }
            /*************not exist************/
            else {
              isexist = false;
               Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => route),
              );
            }
          }
        }
      },
      child: SizedBox(
        width: scw * (124.5 / 415),
        height: sch * (59.2 / 592),
        child: Column(
          children: [
            Container(
              decoration: BoxDecoration(
                color: Colors.white,
                shape: BoxShape.circle,
              ),
              child: Padding(
                padding: const EdgeInsets.all(8.0),
                child: Icon(
                  iconData,
                  size: sch * (17.76 / 592),
                  color: Color(0xffFB9534),
                ),
              ),
            ),
            SizedBox(height: sch * (5.92 / 592)),
            Text(
              text,
              style: TextStyle(
                fontSize: 15.0.sp,
                fontWeight: FontWeight.bold,
                color: Color(0xff063221),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

/**********************Widget for the page indicator***************************/
class PageIndicator extends StatelessWidget {
  final int currentPage;
  final int pageCount;

  PageIndicator({required this.currentPage, required this.pageCount});

  @override
  Widget build(BuildContext context) {
    // Function to calculate height based on screen size
    double hig(double h) => (h / 592) * MediaQuery.of(context).size.height;

    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: List.generate(
        pageCount,
        (index) => AnimatedContainer(
          duration: Duration(milliseconds: 300),
          width: hig(5.92),
          height: hig(5.92),
          margin: EdgeInsets.symmetric(horizontal: 5),
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            color: currentPage == index ? Color(0xff063221) : Colors.grey,
          ),
        ),
      ),
    );
  }
}

/************************Bottom navigation bar widget*****************************/
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
        padding: const EdgeInsets.symmetric(horizontal: 20),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            /*************Message page************/
            _buildIconItem(Icons.message, 'Messages', () {
               Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) =>Messages()),
              );
             
              // Add onTap functionality for Settings
            }),
            /***************Home page*************/
            _buildIconItem(Icons.home, 'Home', () {
             
            }),
            /*************profile page************/
            _buildIconItem(Icons.person, 'Profile', () {
               Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => ProfilePage()),
              );
          
              // Add onTap functionality for Profile
            }),
          ],
        ),
      ),
    );
  }

/****************widget to build item og navigation bar*********************/
  Widget _buildIconItem(IconData iconData, String text, VoidCallback onTap) {
    return GestureDetector(
      onTap: onTap,
      child: SizedBox(
        height: hig(59.2), // Replace with appropriate height function if needed
        child: Center(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(
                iconData,
                size: 30,
                color: text == "Home"
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
