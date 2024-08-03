import 'dart:convert';
import 'dart:io';
import 'package:app/SignUp.dart';
import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:app/profile.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter/widgets.dart';
import 'package:image_picker/image_picker.dart';

class EditProfile extends StatefulWidget {
  @override
  _EditProfileState createState() => _EditProfileState();
}

class _EditProfileState extends State<EditProfile> {
  String NoError = '';
  String nameChange = user.username;
  String emailChange = user.email;
  String? passwordChange = null;
  String phoneNumberChange = user.phoneNumber;
  String locationChange = user.location;
  String systemSizeChange = user.systemsize!;
  String cityChange = user.city;
  String currentImage = user.image;
  String newProfileImagePath = ''; // To store the picked image path
  bool updateimage = false;
  double screenWidth = 0;
  double screenHeight = 0;
  double textScale = 0;
  bool clicked = false;
  bool valid = true;
  bool loadingsave = false;

  final ImagePicker _picker = ImagePicker(); // Initialize ImagePicker

  TextEditingController nameController =
      TextEditingController(text: user.username);
  TextEditingController emailController =
      TextEditingController(text: user.email);
  TextEditingController passwordController = TextEditingController(text: '');
  TextEditingController phoneController =
      TextEditingController(text: user.phoneNumber);
  TextEditingController locationController =
      TextEditingController(text: user.location);
  TextEditingController systemSizeController =
      TextEditingController(text: user.systemsize);
  TextEditingController CityController = TextEditingController(text: user.city);

  @override
  void dispose() {
    nameController.dispose();
    emailController.dispose();
    passwordController.dispose();
    phoneController.dispose();
    locationController.dispose();
    systemSizeController.dispose();
    super.dispose();
  }

/************************main widget****************************/

  @override
  Widget build(BuildContext context) {
    screenWidth = MediaQuery.of(context).size.width;
    screenHeight = MediaQuery.of(context).size.height;
    textScale = MediaQuery.of(context).textScaleFactor;

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
                    /*********************Header**********************/
                    buildTopBar(),
                    /********************User Image*********************/
                    buildProfileImage(),
                    SizedBox(height: scaledHeight(10)),
                    /***********************user Data**************************/
                    buildTextField('Name', nameController),
                    SizedBox(height: scaledHeight(15)),
                    buildTextField('Email', emailController),
                    SizedBox(height: scaledHeight(15)),
                    buildTextField('Password', passwordController),
                    SizedBox(height: scaledHeight(15)),
                    buildTextField('Phone Number', phoneController),
                    SizedBox(height: scaledHeight(15)),
                    buildTextField('Location', locationController),
                    SizedBox(height: scaledHeight(15)),
                    buildTextField('System Size', systemSizeController),
                    SizedBox(height: scaledHeight(15)),
                    builddropDown("City", CityController),
                    SizedBox(height: scaledHeight(15)),
                    /************************Error message******************************/
                    if (clicked && !valid) builderrorinputtext(),
                    if (clicked && !valid) SizedBox(height: scaledHeight(15)),
                    /************************** Buttons ************************/
                    buildActionButtons(),
                    SizedBox(height: scaledHeight(15)),
                  ],
                ),
              ),
            ),
            if (loadingsave)
              const Opacity(
                  opacity: 0.8,
                  child: ModalBarrier(dismissible: false, color: Colors.black)),
            if (loadingsave)
              const Center(
                child: CircularProgressIndicator(),
              )
          ],
        ),
      ),
    );
  }

/********************first part of screen***********************/
  Widget buildTopBar() {
    return SizedBox(
      height: scaledHeight(89),
      width: double.infinity,
      child: Stack(
        children: [
          Positioned(
            top: 0,
            right: 0,
            child: Image.asset(
              'assets/editprofile/cloud.png',
              fit: BoxFit.fill,
              width: scaledWidth(142.5),
              height: scaledHeight(80),
            ),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              GestureDetector(
                onTap: () {
                  Navigator.pushReplacement(
                    context,
                    MaterialPageRoute(builder: (context) => ProfilePage()),
                  );

                },
                child: Image.asset(
                  'assets/editprofile/arrow.png',
                  width: scaledWidth(71.2),
                  height: scaledHeight(71),
                ),
              ),
              Text(
                'Edit Profile',
                style: TextStyle(
                  fontSize: 25.0 * textScale,
                  fontWeight: FontWeight.bold,
                  color: Color(0x0ff063221),
                ),
              ),
              SizedBox(width: scaledWidth(71.2)),
            ],
          ),
        ],
      ),
    );
  }

/****************image of profile********************/

  Widget buildProfileImage() {
    return SizedBox(
      width: scaledWidth(123.2),
      height: scaledHeight(90),
      child: Stack(
        children: [
          Center(
            child: ClipOval(
              child: SizedBox(
                  width: scaledWidth(123.2),
                  height: scaledHeight(90),
                  child: newProfileImagePath.isEmpty
                      /****************not select new image*******************/
                      ? Image.network(
                          currentImage,
                          width: scaledWidth(123.2),
                          height: scaledHeight(90),
                          fit: BoxFit.cover,
                          loadingBuilder: (BuildContext context, Widget child,
                              ImageChunkEvent? loadingProgress) {
                            if (loadingProgress == null) return child;
                            return Center(
                              child: CircularProgressIndicator(
                                value: loadingProgress.expectedTotalBytes !=
                                        null
                                    ? loadingProgress.cumulativeBytesLoaded /
                                        loadingProgress.expectedTotalBytes!
                                    : null,
                              ),
                            );
                          },
                          errorBuilder: (BuildContext context, Object exception,
                              StackTrace? stackTrace) {
                            return Text('Error loading image');
                          },
                        )
                      /**************** select new image not delete*******************/
                      : newProfileImagePath !=
                              hosturl+"ProfileImages/profile.png"
                          ? Image.file(
                              File(newProfileImagePath),
                              width: scaledWidth(123.2),
                              height: scaledHeight(90),
                              fit: BoxFit.cover,
                            )
                          /**************** delete image*******************/
                          : Image.network(
                              newProfileImagePath,
                              width: scaledWidth(123.2),
                              height: scaledHeight(90),
                              fit: BoxFit.cover,
                              loadingBuilder: (BuildContext context,
                                  Widget child,
                                  ImageChunkEvent? loadingProgress) {
                                if (loadingProgress == null) return child;
                                return Center(
                                  child: CircularProgressIndicator(
                                    value: loadingProgress.expectedTotalBytes !=
                                            null
                                        ? loadingProgress
                                                .cumulativeBytesLoaded /
                                            loadingProgress.expectedTotalBytes!
                                        : null,
                                  ),
                                );
                              },
                              errorBuilder: (BuildContext context,
                                  Object exception, StackTrace? stackTrace) {
                                return Text('Error loading image');
                              },
                            )),
            ),
          ),
          /************popuu for update or delete image*************/
          Positioned(
            right: 0,
            bottom: 0,
            child: Container(
                width: scaledWidth(40),
                height: scaledHeight(42),
                child: PopupMenuButton<String>(
                    icon: CircleAvatar(
                      radius: 90,
                      backgroundColor: Color(0x0ffFF914D),
                      child: Icon(
                        Icons.edit,
                        size: 12,
                        color: Colors.white,
                      ),
                    ),
                    onSelected: (String result) async {
                      if (result == 'update') {
                        await pickImage();
                        setState(() {
                          if (imageFile != null) updateimage = true;
                        });
                      } else if (result == 'delete') {
                        setState(() {
                          updateimage = true;
                          imageFile = null;
                          newProfileImagePath =
                              hosturl+"ProfileImages/profile.png";
                        });
                      }
                    },
                    itemBuilder: (BuildContext context) =>
                        <PopupMenuEntry<String>>[
                          const PopupMenuItem<String>(
                            value: 'update',
                            child: Text('Update Image'),
                          ),
                          const PopupMenuItem<String>(
                            value: 'delete',
                            child: Text('Delete Image'),
                          ),
                        ])),
          ),
        ],
      ),
    );
  }

/******************function onpress on button**************************/
  File? imageFile;
  Future<void> pickImage() async {
    final pickedFile = await _picker.pickImage(source: ImageSource.gallery);
    if (pickedFile != null) {
      setState(() {
        imageFile = File(pickedFile.path);
        newProfileImagePath = pickedFile.path;
      });
    }
  }

/*********************widget build dropdown*********************/
  Widget builddropDown(String label, TextEditingController controller) {
    return Column(
      children: [
        Row(
          children: [
            SizedBox(width: screenWidth * 0.129),
            Icon(
              size: 15,
              getIcon(label),
              color: Color(0x0ff063221),
            ),
            SizedBox(width: screenWidth * 0.032),
            Text(
              label,
              style: TextStyle(
                fontSize: 12.0 * textScale,
                fontWeight: FontWeight.bold,
                color: Color(0x0ff063221),
              ),
            ),
          ],
        ),
        SizedBox(height: screenHeight * 0.01689),
        SizedBox(
            height: screenHeight * 0.042,
            width: screenWidth * 0.71,
            child: DropdownButtonFormField<String>(
              value: controller.text.isEmpty ? null : controller.text,
              items: cities.map((String value) {
                return DropdownMenuItem<String>(
                  value: value,
                  child: Text(value),
                );
              }).toList(),
              onChanged: (String? newValue) {
                setState(() {
                  controller.text = newValue!;
                  updateUser(label, newValue);
                });
              },
              decoration: InputDecoration(
                contentPadding: EdgeInsets.symmetric(vertical: 0),
                labelStyle: TextStyle(color: Colors.grey),
                filled: true,
                fillColor: Color.fromARGB(255, 226, 234, 221),
                enabledBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10),
                  borderSide: BorderSide(color: Colors.transparent),
                ),
                focusedBorder: OutlineInputBorder(
                  borderRadius: BorderRadius.circular(10),
                  borderSide: BorderSide(color: Colors.transparent),
                ),
              ),
            )),
      ],
    );
  }

/***********************widget build text field******************************/
  Widget buildTextField(String label, TextEditingController controller) {
    return Column(
      children: [
        Row(
          children: [
            SizedBox(width: screenWidth * 0.129),
            Icon(
              size: 15,
              getIcon(label),
              color: Color(0x0ff063221),
            ),
            SizedBox(width: screenWidth * 0.032),
            Text(
              label,
              style: TextStyle(
                fontSize: 12.0 * textScale,
                fontWeight: FontWeight.bold,
                color: Color(0x0ff063221),
              ),
            ),
          ],
        ),
        SizedBox(height: screenHeight * 0.01689),
        SizedBox(
          height: screenHeight * 0.042,
          width: screenWidth * 0.71,
          child: TextField(
            style: TextStyle(
              fontSize: 12.0 * textScale,
              color: Colors.black,
            ),
            controller: controller,
            onChanged: (value) {
              setState(() {
                updateUser(label, value);
              });
            },
            keyboardType: label == "System Size"
                ? TextInputType.numberWithOptions(decimal: true)
                : TextInputType.text,
            inputFormatters: label == "System Size"
                ? [
                    FilteringTextInputFormatter.allow(RegExp(r'^(\d*\.?\d*)$')),
                  ]
                : [],
            textAlignVertical: TextAlignVertical.center,
            obscureText: label=="Password"?true:false,
            decoration: InputDecoration(
              contentPadding: EdgeInsets.symmetric(vertical: 0),
              labelStyle: TextStyle(color: Colors.grey),
              filled: true,
              fillColor: Color.fromARGB(255, 226, 234, 221),
              enabledBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(10),
                borderSide: BorderSide(color: Colors.transparent),
              ),
              focusedBorder: OutlineInputBorder(
                borderRadius: BorderRadius.circular(10),
                borderSide: BorderSide(color: Colors.transparent),
              ),
            ),
          ),
        ),
      ],
    );
  }

/***************onchange function of textfield*******************/
  void updateUser(String label, String value) {
    switch (label) {
      case 'Name':
        nameChange = value;
        break;
      case 'Email':
        emailChange = value;
        break;
      case 'Password':
        passwordChange = value;
        break;
      case 'Phone Number':
        phoneNumberChange = value;
        break;
      case 'Location':
        locationChange = value;
        break;
      case 'System Size':
        systemSizeChange = value;
      case 'City':
        cityChange = value;
        break;
    }
  }

/********************widget that build buttons********************/
  Widget buildActionButtons() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Padding(
          padding: EdgeInsets.only(left: scaledWidth(12.5)),
          child: SizedBox(
            width: scaledWidth(124),
            height: scaledWidth(25),
            child: ElevatedButton(
              onPressed: () {
                Navigator.pushReplacement(
                  context,
                  MaterialPageRoute(builder: (context) => ProfilePage()),
                );

              },
              child: Text(
                'Cancel',
                style: TextStyle(
                  fontSize: 15 * textScale,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
              style: ElevatedButton.styleFrom(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(20),
                ),
                backgroundColor: Colors.grey,
              ),
            ),
          ),
        ),
        /**********************************Save button*******************************/
        Padding(
          padding: EdgeInsets.only(right: scaledWidth(12.5)),
          child: SizedBox(
            width: scaledWidth(124),
            height: scaledHeight(25),
            child: ElevatedButton(
              onPressed: () async {
                /************if all data entered***************/
                if (!nameChange.isEmpty &&
                    !emailChange.isEmpty &&
                    !phoneNumberChange.isEmpty &&
                    !systemSizeChange.isEmpty &&
                    !locationChange.isEmpty) {
                  setState(() {
                    loadingsave = true;
                  });
                  /*********call api************/
                  NoError = await user.updateUserData(
                      imageFile,
                      nameChange,
                      emailChange,
                      passwordChange,
                      phoneNumberChange,
                      locationChange,
                      systemSizeChange,
                      cityChange,
                      updateimage);
                  setState(() {
                    loadingsave = false;
                  });
                  /**********if response ok*************/
                  if (NoError == 'Ok') {
                    setState(() {
                      clicked = true;
                      valid = true;
                    });
                    Navigator.pushReplacement(
                      context,
                      MaterialPageRoute(builder: (context) => ProfilePage()),
                    );
                  }
                  /*************if response not ok*************/ 
                  else {
                    setState(() {
                      clicked = true;
                      valid = false;
                    });
                  }
                }
                /*************if there is empty data**************/ 
                else {
                  if (nameChange.isEmpty) {
                    NoError = "Please Enter Name";
                  } else if (emailChange.isEmpty) {
                    NoError = "Please Enter Email";
                  } else if (phoneNumberChange.isEmpty) {
                    NoError = "Please Enter PhoneNumber";
                  } else if (locationChange.isEmpty) {
                    NoError = "Please Enter Location";
                  } else {
                    NoError = "Please Enter System Size";
                  }
                  setState(() {
                    clicked = true;
                    valid = false;
                  });
                }
              },
              child: Text(
                'Save',
                style: TextStyle(
                  fontSize: 15 * textScale,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
              style: ElevatedButton.styleFrom(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(20),
                ),
                backgroundColor: Color(0x0ffFF914D),
              ),
            ),
          ),
        ),
      ],
    );
  }

/****************if data null************************/
  Widget builderrorinputtext() {
    return Text(
      NoError,
      style: TextStyle(color: Colors.red, fontSize: 13),
    );
  }

  IconData getIcon(String label) {
    switch (label) {
      case 'Email':
        return Icons.email;
      case 'Password':
        return Icons.password;
      case 'Phone Number':
        return Icons.phone;
      case 'Location':
        return Icons.location_city;
      case 'System Size':
        return Icons.system_update;
      default:
        return Icons.person;
    }
  }

  double scaledWidth(double width) {
    return screenWidth * (width / 385);
  }

  double scaledHeight(double height) {
    return (height / 592) * screenHeight;
  }
}
