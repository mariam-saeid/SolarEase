import 'package:app/MarketPlace/APIService/PostModel.dart';
import 'package:app/MarketPlace/APIService/SmallPostModel.dart';

import 'package:app/main.dart';
import 'package:app/whenclick.dart';

import 'package:dio/dio.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';
import 'package:flutter/widgets.dart';

class CardWidet extends StatefulWidget {
  Smallpostmodel smallpostmodel;
  CardWidet({
    required this.smallpostmodel,
  });

  @override
  _CardWidetState createState() => _CardWidetState();
}

class _CardWidetState extends State<CardWidet> {
  // bool isFavorite = false;

  UserPostService userPostService = UserPostService(Dio());

  @override
  Widget build(BuildContext context) {
    //color: Color(0xffcdefbd),
    return SizedBox(
      height: height(180),
      width: width(370),
      child: InkWell(
        onTap: () {
          Navigator.of(context).push(MaterialPageRoute(
              builder: (_) => Postpage(
                    postId: widget.smallpostmodel.PostID,
                  )));
        },
        child: Card(
          color: Color(0xffD9EDCA),
          //color: Color(0xffD9EDCA), //#D9EDCA
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.only(left: 8, top: 14),
                child:
                    _UserProfileBuilder(smallpostmodel: widget.smallpostmodel),
              ),
              Align(
                  alignment: Alignment.center,
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8),
                    child: _buildImage(smallpostmodel: widget.smallpostmodel),
                  )),
              Padding(
                padding: const EdgeInsets.only(left: 16, top: 12),
                child: Row(
                  children: [
                    _TextBuilder(smallpostmodel: widget.smallpostmodel),
                  ],
                ),
              ),
              Align(
                alignment: Alignment.topRight,
                child: IconButton(
                  onPressed: () async {
                    setState(() {
                      widget.smallpostmodel.isFavorite =
                          !widget.smallpostmodel.isFavorite;
                      //isFavorite = !isFavorite;
                      // add to favorite
                    });
                    print('jnhsCJHD');

                    await userPostService?.AddFvoritePost(
                        widget.smallpostmodel.PostID);
                    print(widget.smallpostmodel.PostID);
                  },
                  icon: Icon(
                    widget.smallpostmodel.isFavorite
                        ? Icons.favorite
                        : Icons.favorite_border,
                    color: widget.smallpostmodel.isFavorite
                        ? Colors.orange
                        : Colors.black,

                    // isFavorite ? Icons.favorite : Icons.favorite_border,
                    // color: isFavorite ? Colors.orange : Colors.black,
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

Widget _TextBuilder({required smallpostmodel}) {
  return Expanded(
    child: SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            smallpostmodel.ProductName,
            style: TextStyle(fontSize: 14, fontWeight: FontWeight.bold),
          ),
          Row(
            children: [
              Icon(
                Icons.monetization_on,
                size: 16,
              ),
              Text(smallpostmodel.ProductPrice,
                  style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14)),
            ],
          ),
          Row(
            children: [
              Icon(
                Icons.location_on,
                size: 16,
              ),
              Text(smallpostmodel.City,
                  style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14)),
            ],
          ),
        ],
      ),
    ),
  );
}
// ------------------------------------------------------

SizedBox _buildImage({required smallpostmodel}) {
  return SizedBox(
    width: width(300),
    height: height(60),
    child: ClipRRect(
      borderRadius: BorderRadius.all(Radius.circular(35)),
      child: Image.network(
          fit: BoxFit.fill,
          "http://solareasegp.runasp.net/" + smallpostmodel.ProductImage),

      //child: Image.asset(smallpostmodel.ProductImage, fit: BoxFit.fill,),
    ),
  );
}

// ----------------------------------------------------
Widget _UserProfileBuilder({required smallpostmodel}) {
  return Row(
    children: [
      CircleAvatar(
        radius: 25,

        backgroundImage: NetworkImage("http://solareasegp.runasp.net/" +
                smallpostmodel.UserProfileImage ??
            'assets/N.png'),

        //backgroundImage: AssetImage(smallpostmodel.UserProfileImage?? 'assets/N.png'),
      ),
      Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Padding(
            padding: const EdgeInsets.only(left: 8),
            child: SizedBox(
              width: width(190),
              child: SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                child: Text(
                  smallpostmodel.UserName,
                  style: TextStyle(fontSize: 14, fontWeight: FontWeight.bold),
                ),
              ),
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(left: 8),
            child: Text(
              smallpostmodel.date,
              style: TextStyle(color: Colors.grey),
            ),
          )
        ],
      )
    ],
  );
}
// ----------------------------------------------
