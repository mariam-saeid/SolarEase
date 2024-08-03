//import 'package:app/MarketPlace/ProductView/ProductPrice.dart';
import 'package:app/favorite/PostView/body.dart';
import 'package:app/favorite/ProductView/ProductPrice.dart';
import 'package:app/profile.dart';
import 'package:flutter/material.dart';

double scw = 0; // 60% of screen width
double sch = 0;
double t = 0;

double width(double w) {
  return scw * (w / 783);
}

double height(double h) {
  return (h / 393) * sch;
}

// void main() {
//   runApp(FavoritePage());
// }

class FavoritePage extends StatefulWidget {
  const FavoritePage({Key? key}) : super(key: key);

  @override
  State<FavoritePage> createState() => _MarketPlaceState();
}

class _MarketPlaceState extends State<FavoritePage> {
  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width;
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      home: DefaultTabController(
        length: 2, // Number of tabs
        child: WillPopScope(
          onWillPop: () async {
            return false;
          },
          child: Scaffold(
            appBar: AppBar(
              forceMaterialTransparency: true,
              flexibleSpace: SizedBox(
                height: height(150),
                width: double.infinity,
                child: Center(
                  child: Stack(
                    children: [
                      Positioned(
                        top: 0,
                        right: 0,
                        child: Image.asset(
                          'assets/favorite/cloud.png',
                          fit: BoxFit.fill,
                          width: width(350),
                          height: height(60),
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
                              print('Arrow tapped');
                            },
                            child: Padding(
                              padding: const EdgeInsets.only(left: 16),
                              child: Image.asset(
                                'assets/favorite/arrow.png',
                                width: width(150),
                                height: height(150),
                              ),
                            ),
                          ),
                          Text(
                            'Favorites',
                            style: TextStyle(
                              fontSize: 25.0 * t,
                              fontWeight: FontWeight.bold,
                              color: Color(0xff063221),
                            ),
                          ),
                          SizedBox(
                            width: width(166),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
              ),
              bottom: TabBar(
                indicatorColor: Colors.orange,
                dividerColor: Colors.transparent,
                tabs: [
                  Tab(
                    child: Text(
                      'Exchange Items',
                      style: TextStyle(
                        fontSize: 18,
                        color: Colors.black,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                  Tab(
                    child: Text(
                      'Products',
                      style: TextStyle(
                        fontSize: 18,
                        color: Colors.black,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ],
              ),
            ),
            body: Padding(
              padding: const EdgeInsets.only(top: 8.0),
              child: TabBarView(
                children: [
                  FavoritBody(),
                  ProductPrices(),
          
                  ///
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
