import 'package:app/Marketplace/MarketPlace.dart';
import 'package:app/api_objects/post.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

/*****************Global variables******************/
double scw = 0;
double sch = 0;
double t = 0;

Post post = Post(
    brandChange: "brandChange",
    capacityChange: 5,
    categoryChange: "categoryChange",
    cityChange: "cityChange",
    descriptionChange: "",
    imageFile: null,
    isusedChange: true,
    locationChange: "locationChange",
    priceChange: 5,
    unitChange: "unitChange",
    voltChange: 5);

double wid(BuildContext context, double w) {
  return MediaQuery.of(context).size.width * (w / 360);
}

double hig(BuildContext context, double h) {
  return (h / 592) * MediaQuery.of(context).size.height;
}

/********************************************************/
class Postpage extends StatefulWidget {
  int postId;
  Postpage({required this.postId});

  @override
  State<Postpage> createState() => _PostpageState();
}

/******************Main widget******************/
class _PostpageState extends State<Postpage> {
  String NoError = "Ok"; //save response result
  bool isloadingbigPost = true;
  @override
  void initState() {
    super.initState();
    getpost();
  }

  /************Get post to show based on ID**************/
  Future<void> getpost() async {
    setState(() {
      isloadingbigPost = true;
    });
    NoError = await post.getPost(widget.postId);
    setState(() {
      isloadingbigPost = false;
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
              child: Column(
                children: <Widget>[
                  SizedBox(
                    height: hig(context, 100),
                    width: double.infinity,
                    child: Stack(
                      children: [
                        /************top image*************/
                        Positioned(
                          top: 0,
                          right: 0,
                          child: Image.asset(
                            'assets/whenclick/cloud.png',
                            fit: BoxFit.fill,
                            width: wid(context, 90),
                            height: hig(context, 90),
                          ),
                        ),
                        /************arrow and title*************/
                        Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: [
                                GestureDetector(
                                  onTap: () {
                                    Navigator.pop(context);
                                    print('Arrow tapped');
                                  },
                                  child: Image.asset(
                                    'assets/whenclick/arrow.png',
                                    width: wid(context, 70),
                                    height: hig(context, 80),
                                  ),
                                ),
                                Text(
                                  'Post',
                                  style: TextStyle(
                                    fontSize: 25.0 * t,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0xff063221),
                                  ),
                                ),
                                SizedBox(
                                  width: wid(context, 70),
                                ),
                              ],
                            ),
                            /********if response not ok show error message***********/
                            if (NoError != "Ok")
                              Center(
                                child: Text(
                                  "Oops! Something went wrong.",
                                  style: TextStyle(
                                      fontSize: 15.0, color: Colors.red),
                                ),
                              )
                          ],
                        ),
                      ],
                    ),
                  ),
                  /***********post to show****************/
                  Postdinfo(),
                  SizedBox(
                    height: hig(context, 10),
                  )
                ],
              ),
            ),
            /**************Waitting response*****************/
            if (isloadingbigPost)
              const Opacity(
                  opacity: 0.8,
                  child: ModalBarrier(dismissible: false, color: Colors.black)),
            if (isloadingbigPost)
              const Center(
                child: CircularProgressIndicator(),
              )
          ],
        ),
      ),
    );
  }
}

/***************widget show post information*******************/
class Postdinfo extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Container(
      width: wid(context, 340),
      /************height based on category***********/
      height: post.categoryChange == "Battery"
          ? hig(context, 532)
          : hig(context, 512),
      decoration: BoxDecoration(
        color: Color(0xffE1F1D5),
        borderRadius: BorderRadius.circular(20),
      ),
      child: Center(
        child: SizedBox(
          width: wid(context, 320),
          height: post.categoryChange == "Battery"
              ? hig(context, 522)
              : hig(context, 502),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  /***********owner image**************/
                  ClipOval(
                    child: Image.network(
                      post.ownerPhoto!,
                      width: wid(context, 39),
                      height: hig(context, 39),
                    ),
                  ),
                  SizedBox(width: wid(context, 15)),
                  /***********owner name**************/
                  Text(
                    post.ownerName ?? "User",
                    style: TextStyle(
                      fontSize: 18.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Color(0xff063221),
                    ),
                  ),
                  Spacer(flex: 5),
                  /************date*************/
                  Text(
                    post.date ?? "-/-/-",
                    style: TextStyle(
                      fontSize: 15.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Color(0xff063221),
                    ),
                  ),
                  Spacer(flex: 1),
                ],
              ),
              SizedBox(height: hig(context, 10)),
              /*********post image***********/
              Center(
                child: Image.network(
                  post.photo!,
                  fit: BoxFit.fill,
                  width: wid(context, 200),
                  height: hig(context, 120),
                ),
              ),
              SizedBox(height: hig(context, 10)),
              /*************is used field**************/
              Conmponent(
                w: wid(context, 280),
                h: hig(context, 35),
                txt: "is used",
                w_: wid(context, 70),
                wt: wid(context, 85),
                s: post.isusedChange ? "Yes" : "No",
              ),
              /*************brand field**************/
              Conmponent(
                w: wid(context, 280),
                h: hig(context, 35),
                txt: "Brand",
                w_: wid(context, 195),
                wt: wid(context, 85),
                s: post.brandChange,
              ),
              /*************Category field**************/
              Conmponent(
                w: wid(context, 280),
                h: hig(context, 35),
                txt: "category",
                w_: wid(context, 195),
                wt: wid(context, 85),
                s: post.categoryChange,
              ),
              /*************if categoory battery show volt**************/
              if (post.categoryChange == 'Battery')
                Conmponent(
                  w: wid(context, 280),
                  h: hig(context, 35),
                  txt: "volt",
                  w_: wid(context, 195),
                  wt: wid(context, 85),
                  s: post.voltChange.toString() + "V",
                ),
              /*************Capacity field**************/
              Conmponent(
                w: wid(context, 280),
                h: hig(context, 35),
                txt: "Capacity",
                w_: wid(context, 195),
                wt: wid(context, 85),
                s: post.capacityChange.toString() + post.unitChange,
              ),
              /****************price field*****************/
              Conmponent(
                w: wid(context, 280),
                h: hig(context, 35),
                txt: "Price",
                w_: wid(context, 195),
                wt: wid(context, 85),
                s: post.priceChange.toString(),
              ),
              /*************phone field**************/
              Conmponent(
                w: wid(context, 280),
                h: hig(context, 35),
                txt: "Phone",
                w_: wid(context, 195),
                wt: wid(context, 85),
                s: post.phone ?? "",
              ),
              /*************location field**************/
              Conmponent(
                w: wid(context, 305),
                h: hig(context, 35),
                txt: "Location",
                w_: wid(context, 220),
                wt: wid(context, 85),
                s: post.locationChange.toString() + " ," + post.cityChange,
              ),
              /*************Discription field**************/
              Text(
                "Description",
                style: TextStyle(
                  fontSize: 15.0 * t,
                  fontWeight: FontWeight.bold,
                  color: Color(0xff063221),
                ),
              ),
              Conmponent(
                w: wid(context, 320),
                h: hig(context, 45),
                txt: "Description",
                w_: wid(context, 320),
                wt: 0,
                s: post.descriptionChange.toString(),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

/********************field contain info***********************/
class Conmponent extends StatelessWidget {
  final double w;
  final double h;
  final String txt;
  final double w_;
  final double wt;
  final String s;

  Conmponent({
    required this.w,
    required this.h,
    required this.txt,
    required this.w_,
    required this.wt,
    required this.s,
  });

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: w,
      height: h,
      child: Row(
        children: [
          /*************name of field*****************/
          SizedBox(
            width: wt,
            child: Text(
              txt,
              style: TextStyle(
                fontSize: 15.0 * MediaQuery.of(context).textScaleFactor,
                fontWeight: FontWeight.bold,
                color: Color(0xff063221),
              ),
            ),
          ),
          /************Text field*************/
          Container(
            width: w_,
            height: txt == "Description" ? h : hig(context, 20),
            decoration: BoxDecoration(
              color: Color(0xffD9D9D9),
              borderRadius: BorderRadius.circular(20),
            ),
            child: Center(
              child: SingleChildScrollView(scrollDirection: Axis.horizontal,child: Text(s,maxLines: txt == "Description" ? 3 : 1,)),
            ),
          ),
        ],
      ),
    );
  }
}