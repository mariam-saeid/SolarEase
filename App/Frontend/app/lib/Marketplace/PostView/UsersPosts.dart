import 'package:app/MarketPlace/APIService/SmallPostModel.dart';
import 'package:app/MarketPlace/PostView/PostCard.dart';
import 'package:app/MarketPlace/PostView/dropdownlistforpost.dart';
import 'package:app/MarketPlace/ProductView/Search.dart';
import 'package:app/main.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import 'package:dio/dio.dart';

class UsersPosts extends StatefulWidget {
  @override
  _UsersPostsState createState() => _UsersPostsState();
}

class _UsersPostsState extends State<UsersPosts> {
  late Future<List<Smallpostmodel>> futurePosts;
  final smallPostService = SmallPostService(Dio());
  String _selectedCategory = 'all';

  @override
  void initState() {
    super.initState();
    defaultView();
  }

  Future<void> defaultView() async {
    futurePosts = smallPostService.getSmallPosts('', '');
    setState(() {});
  }

  void _onCategoryChanged(String category) async {
    if (category == "all") {
      _selectedCategory = '';
    } else {
      _selectedCategory = category;
    }
    futurePosts = smallPostService.getSmallPosts(_selectedCategory, '');

    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        DownListPost(onCategoryChanged: _onCategoryChanged),
        SearchWidgwt(onpress: (String v) async {
          if (_selectedCategory == "all") {
            _selectedCategory = '';
          } else {
            _selectedCategory = _selectedCategory;
          }
          futurePosts = smallPostService.getSmallPosts(_selectedCategory, v);
          print(futurePosts);
          setState(() {});
        }),
        SizedBox(height: height(6)),
        Expanded(
          child: FutureBuilder<List<Smallpostmodel>>(
            future: futurePosts,
            builder: (context, snapshot) {
              if (snapshot.connectionState == ConnectionState.waiting) {
                return Center(child: CircularProgressIndicator());
              } else if (snapshot.hasError) {
                return Center(child: Text("Error: ${snapshot.error}"));
              } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
                return Center(child: Text("No posts available"));
              } else {
                List<Smallpostmodel> posts = snapshot.data!;
                return ListView.builder(
                  itemCount: (posts.length / 2).ceil(),
                  itemBuilder: (context, index) {
                    int firstIndex = index * 2;
                    int secondIndex = firstIndex + 1;
                    bool hasSecond = secondIndex < posts.length;
                    return Row(
                      children: [
                        PostCard(smallpostmodel: posts[firstIndex]),
                        if (hasSecond)
                          PostCard(smallpostmodel: posts[secondIndex]),
                      ],
                    );
                  },
                );
              }
            },
          ),
        ),
      ],
    );
  }
}
