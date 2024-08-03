import 'dart:io';
import 'dart:typed_data';
import 'package:app/RemovePost/body.dart';
import 'package:app/SignUp.dart';
import 'package:app/api_objects/post.dart';
import 'package:app/api_objects/url.dart';
import 'package:flutter/material.dart';
import 'package:flutter/gestures.dart';
import 'package:image_picker/image_picker.dart';

// Global Variables
double scw = 0; // 60% of screen width
double sch = 0;
double t = 0;
bool editpostloading = false;

String postimage = hosturl+"ProfileImages/profile.png";
File? imageFile;
bool? isusedChange = true;
String? categoryChange = "Inverter";
double? priceChange = 50;
//imageChange;
String? brandChange = "baterry";
double? capacityChange = 50;
String? unitChange = "W";
double? voltChange = null;
String? locationChange = "Cairo";
String cityChange = "Cairo";
String descriptionChange = '';

// Custom Scroll Behavior
class MyCustomScrollBehavior extends MaterialScrollBehavior {
  @override
  Set<PointerDeviceKind> get dragDevices => {
        PointerDeviceKind.touch,
        PointerDeviceKind.mouse,
      };
}

// Utility Functions
double wid(double w) {
  return scw * (w / 420);
}

double hig(double h) {
  return (h / 592) * sch;
}

//////////////////////////////////////
class editPostApp extends StatefulWidget {
  int postid;
  editPostApp({required this.postid});
  @override
  _AddPostScreenState createState() => _AddPostScreenState();
}

class _AddPostScreenState extends State<editPostApp> {
  String getPostError = "Ok";
  Post post = Post(
      brandChange: "brandChange",
      capacityChange: 50,
      categoryChange: "categoryChange",
      cityChange: "cityChange",
      descriptionChange: descriptionChange,
      imageFile: null,
      isusedChange: true,
      locationChange: "locationChange",
      priceChange: 50,
      unitChange: "unitChange",
      voltChange: null);
  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    getPostData();
  }

  Future<void> getPostData() async {
    setState(() {
      editpostloading = true;
    });
    getPostError = await post.getPost(widget.postid);
    postimage = post.photo;
    isusedChange = post.isusedChange;
    categoryChange = post.categoryChange;
    priceChange = post.priceChange;
    brandChange = post.brandChange;
    capacityChange = post.capacityChange;
    unitChange = post.unitChange;
    voltChange = post.voltChange;
    locationChange = post.locationChange;
    cityChange = post.cityChange;
    descriptionChange = post.descriptionChange;

    setState(() {
      editpostloading = false;
    });
  }

  /****************************Update Post data********************************/
  void updatePostDats(String label, String value) {
    switch (label) {
      case 'is used':
        isusedChange = value == "yes" ? true : false;
        break;
      case 'Category':
        categoryChange = value;
        break;
      case 'Capacity':
        capacityChange = value == '' ? null : double.parse(value);
        break;
      case 'Location':
        locationChange = value;
        break;
      case 'City':
        cityChange = value;
        break;
      case 'Brand':
        brandChange = value;
      case 'Unit':
        unitChange = value;

        break;
      case 'Volume':
        voltChange = value == '' ? null : double.parse(value);
        break;
      case 'Price':
        priceChange = value == '' ? null : double.parse(value);
        break;
      case 'Description':
        descriptionChange = value;
      case 'loading':
        setState(() {
          editpostloading = value == "yes" ? true : false;
        });

        break;
    }
  }

/***************************main widget******************************** */
  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width; // 60% of screen width
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;
    return  WillPopScope(
      onWillPop: () async {
        return false;
      },
      child: Scaffold(
        body: Stack(
          children: [
            SingleChildScrollView(
              child: Center(
                child: Column(
                  children: <Widget>[
                    /****************************Header ********************************/
                    Header(getpostError: getPostError),
                    /****************************Input form Post data********************************/
                    PostDataForm(updatePostDats, post.id ?? 0),
                    SizedBox(height: hig(5)),
                  ],
                ),
              ),
            ),
            if (editpostloading)
              const Opacity(
                  opacity: 0.8,
                  child: ModalBarrier(dismissible: false, color: Colors.black)),
            if (editpostloading)
              const Center(
                child: CircularProgressIndicator(),
              )
          ],
        ),
      ),
    );
  }
}

/*********************** Header Widget********************************* */
class Header extends StatelessWidget {
  String getpostError;
  Header({required this.getpostError});
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: hig(100), //=100
      width: double.infinity,
      child: Stack(
        children: [
          Positioned(
            top: 0,
            right: 0,
            child: Image.asset(
              'assets/addpost/cloud.png',
              fit: BoxFit.fill,
              width: wid(140), //=140
              height: hig(90), //=90
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
                        MaterialPageRoute(builder: (context) => RemoveBody()),
                      );

                      print('Arrow tapped');
                    },
                    child: Image.asset(
                      'assets/addpost/arrow.png',
                      width: wid(78),
                      height: hig(70),
                    ),
                  ),
                  Text(
                    'Edit Post',
                    style: TextStyle(
                      fontSize: 25.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Color(0x0ff063221),
                    ),
                  ),
                  SizedBox(
                    width: wid(78),
                  ),
                ],
              ),
              if (getpostError != "Ok")
                Center(
                  child: Text(
                    getpostError,
                    style: TextStyle(color: Colors.red, fontSize: 13),
                  ),
                )
            ],
          ),
        ],
      ),
    );
  }
}

/****************************widget that create input form******************************** */
class PostDataForm extends StatefulWidget {
  void Function(String, String) change;
  PostDataForm(this.change, this.postid);
  int postid;
  @override
  _PostDataFormState createState() => _PostDataFormState();
}

class _PostDataFormState extends State<PostDataForm> {
  String NoError = '';
  Uint8List? _image;
  TextEditingController controller = TextEditingController();
  TextEditingController capacityController =
      TextEditingController(text: capacityChange.toString());
  TextEditingController brandController =
      TextEditingController(text: brandChange.toString());
  TextEditingController priceController =
      TextEditingController(text: priceChange.toString());
  TextEditingController locationController =
      TextEditingController(text: locationChange);
  TextEditingController descriptionController =
      TextEditingController(text: descriptionChange);
  TextEditingController voltController =
      TextEditingController(text: voltChange.toString());
  String? selectedCategory;
  String? selectedBrand;
  String profileImagePath = '';
  bool validdata = true;
  bool clicked = false;

  /****************************Update Post image********************************/
  Widget uploadimage() {
    return GestureDetector(
      onTap: pickImage,
      child: Center(
        child: Image(
          image: _image == null
              ? NetworkImage(postimage)
              : MemoryImage(_image!) as ImageProvider,
          fit: BoxFit.fill,
          width: wid(300),
          height: hig(200), //=120
        ),
      ),
    );
  }

  /***************************check Post data********************************/
  bool databad() {
    return (categoryChange == "Battery" && voltChange == null) |
        (isusedChange == null) |
        (categoryChange == null) |
        (priceChange == null) |
        (brandChange == null || brandChange!.isEmpty) |
        (capacityChange == null) |
        (unitChange == null) |
        (locationChange == null || locationChange!.isEmpty) |
        (cityChange == null || cityChange!.isEmpty);
  }

  /****************************Share button********************************/
  Widget sharebutton() {
    return Expanded(
      child: Align(
        alignment: Alignment.bottomCenter,
        child: SizedBox(
          height: hig(30),
          width: wid(168),
          child: ElevatedButton(
            onPressed: () async {
              setState(() {
                clicked = true;
              });
              /****************if data empty********************/
              if (databad()) {
                if (isusedChange == null) {
                  NoError = "Please Select is Used Field";
                } else if (capacityChange == null) {
                  NoError = "Please Enter Capacity";
                } else if (unitChange == null) {
                  NoError = "Please Select Unit Field";
                } else if (brandChange == null || brandChange!.isEmpty) {
                  NoError = "Please Enter Brand";
                } else if (priceChange == null) {
                  NoError = "Please Enter Price";
                } else if (locationChange == null || locationChange!.isEmpty) {
                  NoError = "Please Enter Locaction";
                } else if (cityChange == null) {
                  NoError = "Please Select City";
                } else {
                  NoError = "Please Enter Volum";
                }

                setState(() {
                  validdata = false;
                });
              } 
              /****************if all data entered*****************/
              else {
                widget.change("loading", "yes");
                Post post = Post(
                    brandChange: brandChange!,
                    capacityChange: capacityChange!,
                    categoryChange: categoryChange!,
                    cityChange: cityChange!,
                    descriptionChange: descriptionChange,
                    imageFile: imageFile,
                    isusedChange: isusedChange!,
                    locationChange: locationChange!,
                    priceChange: priceChange!,
                    unitChange: unitChange!,
                    voltChange: voltChange);
                post.id = widget.postid;
                /************Call api************/
                NoError = await post.edditPost(widget.postid);
  
                widget.change("loading", "No");
                /***********if response ok************/
                 if (NoError == 'Ok') {
                  setState(() {
                    validdata = true;
                  });
                  Navigator.pushReplacement(
                  context,
                  MaterialPageRoute(builder: (context) => RemoveBody() ),
                );
                }
                /*************if not ok************/
                 else {
                  setState(() {
                  });
                }
              }
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Color(0xffED9555), // Set background color
              shape: RoundedRectangleBorder(
                borderRadius:
                    BorderRadius.circular(25), // Set circular border radius
              ),
            ),
            child: Text(
              'Share',
              style: TextStyle(
                color: Colors.white,
                fontWeight: FontWeight.bold,
                // Set text color
                fontSize: 16 * t,
              ),
            ),
          ),
        ),
      ),
    );
  }

  /************build error message*****************/
  Widget builderrorinputtext() {
    return Center(
      child: Text(
        NoError,
        style: TextStyle(color: Colors.red, fontSize: 13),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
     capacityController =
        TextEditingController(text:capacityChange==null?'':capacityChange.toString());
     brandController =
        TextEditingController(text: brandChange.toString());
     priceController =
        TextEditingController(text:priceChange==null?'': priceChange.toString());
     locationController =
        TextEditingController(text: locationChange);
     descriptionController =
        TextEditingController(text: descriptionChange);
     voltController =
        TextEditingController(text:voltChange==null?'':voltChange.toString());
    return Container(
      width: wid(378),
      height: categoryChange == "Battery" ? hig(620) : hig(585), //490
      decoration: BoxDecoration(
        color: Color(0xffE1F1D5),
        borderRadius: BorderRadius.circular(20),
      ),
      child: Center(
        child: SizedBox(
          width: wid(357),
          height: categoryChange == "Battery" ? hig(605) : hig(570),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              SizedBox(height: hig(9)),
              uploadimage(),
              SizedBox(height: hig(9)),
              /****************is used************************/
              DropdownList(wid(357), hig(30), "is used", wid(82), wid(95),
                  ["yes", "no"], widget.change, isusedChange! ? "yes" : "no"),
              /****************Category************************/
              DropdownList(wid(357), hig(30), "Category", wid(156), wid(95), [
                "Solar Panel",
                "Inverter",
                "Battery",
                "Other"
              ], (String lable, String value) {
                setState(() {
                  widget.change("Category", value);
                  selectedCategory = value;
                  widget.change("Brand", '');
                  brandController.text = '';
                  selectedBrand = null;
                  // Reset selected brand when category changes
                });
              }, categoryChange!),
              /****************Volume************************/
              if (categoryChange == 'Battery')
                Conmponent(wid(357), hig(30), "Volume", wid(100), wid(95),
                    widget.change, voltController),

              Row(
                children: [
                  /****************Capacity************************/
                  Conmponent(wid(190), hig(30), "Capacity", wid(90), wid(95),
                      widget.change, capacityController),
                  SizedBox(width: wid(1)),
                  DropdownList(wid(135), hig(30), "Unit", wid(100), wid(35),
                      ["W", "KW", "A"], widget.change, unitChange!),
                ],
              ),
              /****************Brand************************/
              Conmponent(wid(357), hig(30), "Brand", wid(150), wid(95),
                  widget.change, brandController),
              /****************Price************************/
              Conmponent(wid(357), hig(30), "Price", wid(150), wid(95),
                  widget.change, priceController),
              /***********************Location************************/
              Conmponent(wid(357), hig(30), "Location", wid(250), wid(95),
                  widget.change, locationController),
              /****************City************************/
              DropdownList(wid(357), hig(30), "City", wid(150), wid(95),
                  cities, widget.change, cityChange),
              /****************Description************************/
              Text(
                "Description",
                style: TextStyle(
                  fontSize: 15.0 * t,
                  fontWeight: FontWeight.bold,
                  color: Color(0x0ff063221),
                ),
              ), //320
              Conmponent(wid(357), hig(40), "Description", wid(357), 0,
                  widget.change, descriptionController),
              if (NoError != "Ok" && clicked) builderrorinputtext(),
              if (!(NoError != "Ok" && clicked)) SizedBox(height: hig(11)),

              sharebutton()
            ],
          ),
        ),
      ),
    );
  }

/******************function onpress on button************************* */

  final ImagePicker _picker = ImagePicker();

  Future<void> pickImage() async {
    widget.change("loading", "yes");
    final pickedFile = await _picker.pickImage(source: ImageSource.gallery);
    widget.change("loading", "No");
    if (pickedFile != null) {
      widget.change("loading", "yes");
      var imageBytes = await pickedFile.readAsBytes();
      widget.change("loading", "no");
      setState(() {
        imageFile = File(pickedFile.path);
        _image = imageBytes;
        print(imageFile);
      });
    }
  }
}

/***********************Widget for DropDown List**************************** */
class DropdownList extends StatefulWidget {
  final double allWidth;
  final double hig;
  final double widthInput;
  final double widthLable;
  final String lable;
  String initialvalue;

  final List<String> dropdownItems;
  final void Function(String, String) changed;

  DropdownList(this.allWidth, this.hig, this.lable, this.widthInput,
      this.widthLable, this.dropdownItems, this.changed, this.initialvalue);

  @override
  _DropdownListState createState() => _DropdownListState();
}

class _DropdownListState extends State<DropdownList> {
  String? dropdownValue;

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: widget.allWidth,
      height: widget.hig + hig(4),
      child: Row(
        children: [
          SizedBox(
            width: widget.widthLable,
            child: Text(
              widget.lable,
              style: TextStyle(
                fontSize: 15.0 * t,
                fontWeight: FontWeight.bold,
                color: Color(0x0ff063221),
              ),
            ),
          ),
          Container(
            width: widget.widthInput,
            height: widget.hig, //20
            decoration: BoxDecoration(
              color: Color(0xffD9D9D9),
              borderRadius: BorderRadius.circular(20),
            ),
            child: Padding(
              padding: EdgeInsets.symmetric(horizontal: wid(4.2)),
              child: DropdownButtonHideUnderline(
                child: DropdownButton<String>(
                  borderRadius: BorderRadius.circular(20),
                  isDense: true,
                  value: widget.initialvalue,
                  onChanged: (widget.lable == "Category")
                      ? null // Disable dropdown interaction for category field
                      : (newValue) {
                          setState(() {
                            widget.initialvalue = newValue!;
                          });
                          widget.changed(widget.lable, newValue ?? '');
                        },
                  items: widget.dropdownItems
                      .map<DropdownMenuItem<String>>(
                        (String value) => DropdownMenuItem<String>(
                          value: value,
                          child: Text(
                            value,
                            style: TextStyle(
                              fontSize: 13 * t,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                        ),
                      )
                      .toList(),
                  dropdownColor: Color(0xffD9D9D9),
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}

/**************************Widget build Text Field******************************* */
class Conmponent extends StatelessWidget {
  void Function(String, String) change;
  final TextEditingController controller;
  final double w;
  final double h;
  final double w_;
  final double wt;
  final String lable;

  Conmponent(this.w, this.h, this.lable, this.w_, this.wt, this.change,
      this.controller);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: w, //150
      height: h + hig(4), //40
      child: Row(
        children: [
          SizedBox(
            width: wt,
            child: Text(
              lable,
              style: TextStyle(
                fontSize: 15.0 * t,
                fontWeight: FontWeight.bold,
                color: Color(0x0ff063221),
              ),
            ),
          ),
          if (lable != "Description")
            Container(
              width: w_, // Set the width
              height: h, // Set the height
              decoration: BoxDecoration(
                color: Color(0xffD9D9D9), // Set the color
                borderRadius:
                    BorderRadius.circular(20), // Set circular border radius
              ),
              child: Padding(
                padding: EdgeInsets.symmetric(horizontal: w * 0.0106),
                child: TextField(
                  style: TextStyle(
                    fontSize: 13 * t,
                    fontWeight: FontWeight.bold,
                  ),
                  decoration: InputDecoration(
                    isDense: true,
                    border: InputBorder.none, // Remove default border
                  ),
                  controller: controller,
                  onChanged: (value) {
                    change(lable, value);
                  },
                ),
              ),
            )
          else
            Container(
              height: h,
              width: w_, // Set the width
              decoration: BoxDecoration(
                color: Color(0xffD9D9D9), // Set the color
                borderRadius:
                    BorderRadius.circular(15), // Set circular border radius
              ),
              child: TextField(
                decoration: InputDecoration(
                  border: InputBorder.none, // Remove default border
                ),
                controller: controller,
                onChanged: (value) {
                  change(lable, value);
                },
              ),
            ),
        ],
      ),
    );
  }
}
