import 'package:app/api_objects/FindSolar(API).dart';
import 'package:app/homeuser.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';

/**************Global variables***********/
double scw = 0;
double sch = 0;
double t = 0;
String filtervalue = "";
String searchvalue = "";
bool _isloading = true;
List<String> cities = [
  "Nearest",
  "Cairo",
  "Alexandria",
  "Aswan",
  "Asyut",
  "Beheira",
  "Beni Suef",
  "Dakahlia",
  "Damietta",
  "Faiyum",
  "Gharbia",
  "Giza",
  "Ismailia",
  "Kafr el-Sheikh",
  "Marsa Matruh",
  "Minya",
  "Monufia",
  "New Valley",
  "North Sina",
  "Port Said",
  "Qalyubia",
  "Qena",
  "Red Sea",
  "Sharqia",
  "Sohag",
  "South Sina",
  "Suez",
  "Luxor"
];

double wid(var w) {
  return scw * (w / 310);
}

double hig(var h) {
  return (h / 592) * sch;
}

//////////////////////////////////////////
class FindSolarInstallers extends StatefulWidget {
  List<Company> companies = [];
  @override
  _FindSolarInstallersState createState() => _FindSolarInstallersState();
}

class _FindSolarInstallersState extends State<FindSolarInstallers> {
  bool NoError = true;
  final ScrollController controller = ScrollController();
  String dropdownValue = 'Nearest';

  @override
  void initState() {
    super.initState();
    getallcomponies();
  }
/****************Get all companies sorted*****************/
  Future<void> getallcomponies() async {
    setState(() {
      _isloading = true;
    });
    NoError = await getcomponies(componies: widget.companies);
    setState(() {
      _isloading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width;
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;

    return  WillPopScope(
      onWillPop: () async {
        // Return false to disable the back button
        return false;
      },
      child: Scaffold(
        body: Stack(
          children: [
            SingleChildScrollView(
              child: Center(
                child: Column(
                  children: <Widget>[
                    /**************Header**************/
                    buildTopBar(),
                    /*********Dropdown list of componies*********/
                    buildDropdown(),
                    /************Search field****************/
                    buildSearchField(),
                    /***************Companies**************/
                    SizedBox(height: hig(10)),
                    ...widget.companies
                        .map((company) => Column(
                              children: [
                                buildCompany(company),
                                SizedBox(
                                    height:
                                        hig(10)), // Add space after each company
                              ],
                            ))
                        .toList(),
                  ],
                ),
              ),
            ),
            if (_isloading)
              const Opacity(
                  opacity: 0.8,
                  child: ModalBarrier(dismissible: false, color: Colors.black)),
            if (_isloading)
              const Center(
                child: CircularProgressIndicator(),
              )
          ],
        ),
      ),
    );
  }

/*****************divider***********************/
  Widget buildDivider() {
    return Container(color: Colors.grey, height: hig(1), width: wid(200));
  }

/*********************first part of screen*************************/
  Widget buildTopBar() {
    return SizedBox(
      height: hig(100),
      width: double.infinity,
      child: Stack(
        children: [
          Positioned(
            top: 0,
            right: 0,
            child: Image.asset(
              'assets/findsolarinstallers/cloud.png',
              fit: BoxFit.fill,
              width: wid(140),
              height: hig(90),
            ),
          ),
          Column(
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  GestureDetector(
                    onTap: () {
                     Navigator.pushReplacement(
                        context,
                        MaterialPageRoute(builder: (context) => HomeUser(userr: user,)),
                      );

                      print('Arrow tapped');
                    },
                    child: Image.asset(
                      'assets/findsolarinstallers/arrow.png',
                      width: wid(70),
                      height: hig(80),
                    ),
                  ),
                  Text(
                    'Solar Installers',
                    style: TextStyle(
                      fontSize: 25.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Color(0x0ff063221),
                    ),
                  ),
                  SizedBox(width: wid(70)),
                ],
              ),
              if (!NoError)
                Center(
                  child: Text(
                    "Oops! Something went wrong.",
                    style: TextStyle(fontSize: 15.0, color: Colors.red),
                  ),
                )
            ],
          ),
        ],
      ),
    );
  }

/*****************dropdown list that contain cities***********************/
  Widget buildDropdown() {
    return SizedBox(
      width: wid(125),
      height: hig(50),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.start,
        children: [
          SizedBox(
            height: hig(30),
            width: wid(100),
            child: DropdownButtonHideUnderline(
              child: DropdownButton<String>(
                focusColor: Color(0xffFAFAFA),
                dropdownColor: Color(0xffFAFAFA),
                elevation: 0,
                value: dropdownValue,
                icon: Icon(Icons.arrow_drop_down, color: Colors.orange),
                onChanged: (String? newValue) {
                  setState(() {
                    dropdownValue = newValue!;
                  });
                },
                items: cities.map((String value) {
                  return DropdownMenuItem<String>(
                    value: value,
                    onTap: () async {
                      setState(() {
                        filtervalue = value;
                        _isloading = true;
                      });
                      if (value == "Nearest") {
                        filtervalue = '';
                        NoError = await getcomponies(
                            componies: widget.companies, search: searchvalue);
                      } else {
                        NoError = await getcomponies(
                            componies: widget.companies,
                            filter: value,
                            search: searchvalue); 
                      }

                      setState(() {
                        _isloading = false;
                      });
                    },
                    child: Align(
                      alignment: Alignment.center,
                      child: Text(
                        value,
                        textAlign: TextAlign.center,
                        style: TextStyle(
                          fontSize: 15.0 * t,
                          fontWeight: FontWeight.bold,
                          color: Color(0x0ff063221),
                        ),
                      ),
                    ),
                  );
                }).toList(),
              ),
            ),
          ),
          Container(
            width: wid(140),
            height: hig(3),
            color: Color(0x0ffBBBAB6),
          )
        ],
      ),
    );
  }

/*******************search bar************************/
  Widget buildSearchField() {
    return SizedBox(
      width: wid(220),
      height: hig(25),
      child: TextField(
        onTap: () async {
          print("insize" + searchvalue);
        },
        decoration: InputDecoration(
          hintText: 'Search...',
          filled: true,
          fillColor: Color(0xffD9EDCA),
          suffixIcon: Container(
            width: 5,
            height: 5,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: Color(0xffED9555),
            ),
            child: GestureDetector(
              onTap: () async {
                setState(() {
                  _isloading = true;
                });
                print("you click on serch " + filtervalue+ searchvalue);
                /***********call api*********/
                var res = await getcomponies(
                    componies: widget.companies,
                    filter: filtervalue,
                    search: searchvalue);
                setState(() {
                  _isloading = false;
                });
              },
              child: Icon(
                Icons.search,
                size: wid(17),
                color: Colors.white,
              ),
            ),
          ),
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(20.0),
            borderSide: BorderSide.none,
          ),
          contentPadding:
              EdgeInsets.symmetric(vertical: 0, horizontal: wid(10)),
        ),
        style: TextStyle(
          fontSize: 14 * t,
        ),
        textAlign: TextAlign.center,
        onChanged: (value) {
          searchvalue = value;
          print('Search query: $value');
        },
      ),
    );
  }

/*************************build compony card***************************/
  Widget buildCompany(Company company) {
    return Center(
      child: Container(
        width: wid(297),
        decoration: BoxDecoration(
          color: Color(0xffD9EDCA),
          borderRadius: BorderRadius.circular(13),
        ),
        child: Row(
          children: [
            Padding(
              padding: EdgeInsets.only(left: wid(7), right: wid(3)),
              child: Container(
                width: wid(60),
                height: hig(50),
                decoration: BoxDecoration(
                  color: Color(0xff063221),
                  borderRadius: BorderRadius.circular(20),
                ),
                child: Icon(
                  Icons.business,
                  size: 35 * t,
                  color: Colors.white,
                ),
              ),
            ),
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Padding(
                  padding: EdgeInsets.only(
                      left: wid(10), top: hig(10), right: wid(10)),
                  child: Text(
                    company.name,
                    style: TextStyle(
                      color: Color(0xff063221),
                      fontSize: 15 * t,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
                Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    /*************Phone************/
                    buildContactInfo(Icons.phone, company.phone[0]),
                    buildDivider(),

                    /*************email************/
                    ...company.email
                        .map((email) => buildContactInfo(Icons.email, email)),
                    buildDivider(),

                    /*************location if long************/
                    if (company.location[0].length >= 35)
                      buildContactInfo(Icons.location_on,
                          company.location.first.substring(0, 35)),
                    if (company.location[0].length >= 35)
                      buildContactInfo(
                          null, company.location.first.substring(35)),
                    /***********location if short***********/
                    if (company.location[0].length < 35)
                      buildContactInfo(
                          Icons.location_on, company.location.first),
                  ],
                ),
                SizedBox(height: hig(5)),
              ],
            ),
          ],
        ),
      ),
    );
  }

/**************************build raw that cotain data***************************/
  Widget buildContactInfo(IconData? icon, String text) {
    return Center(
      child: Padding(
        padding: EdgeInsets.only(left: wid(5), top: hig(5), right: wid(5)),
        child: Row(
          children: [
            if (icon != null)
              Icon(
                icon,
                size: 15 * t,
                color: Color(0xff063221),
              ),
            if (icon == null)
              Icon(
                icon,
                size: 15 * t,
                color: Color(0xffD9EDCA),
              ),
            if (text.length > 90)
              (Text(
                text.substring(0, 90),
                style: TextStyle(
                  color: Color(0xff063221),
                  fontSize: 13 * t,
                  fontWeight: FontWeight.bold,
                ),
              )),
            Text(
              '  $text',
              style: TextStyle(
                color: Color(0xff063221),
                fontSize: 13 * t,
                fontWeight: FontWeight.bold,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
