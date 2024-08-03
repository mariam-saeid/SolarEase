import 'package:app/RemovePost/APIForRemovePosts/removedpost.dart';
import 'package:app/RemovePost/removeCard.dart';
import 'package:app/profile.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

class RemoveBody extends StatefulWidget {
  @override
  _RemoveBodyState createState() => _RemoveBodyState();
}

class _RemoveBodyState extends State<RemoveBody> {
  final RemovedService _removedService = RemovedService(Dio());
  late Future<List<RemovedPost>> _allPostsFuture;

  @override
  void initState() {
    super.initState();
    _allPostsFuture = _removedService.getAllPosts();
  }

  void _deletePost(int id) async {
    try {
      await _removedService.deletePost(id);
      setState(() {
        // Refresh the list of posts after deletion
        _allPostsFuture = _removedService.getAllPosts();
      });
    } catch (e) {
      print('Failed to delete post: $e');
    }
  }

  void updatepage() async {
    try {
      _allPostsFuture = _removedService.getAllPosts();
    } catch (e) {
      print('Failed to catch new posts: $e');
    }
  }

  @override
  Widget build(BuildContext context) {
    return  WillPopScope(
      onWillPop: () async {
        // Return false to disable the back button
        return false;
      },
      child: Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: false,
           backgroundColor: Color(0xffFFFBFE), 
          flexibleSpace: SizedBox(
            child: Center(
              child: Stack(
                children: [
                  Positioned(
                    top: 0,
                    right: 0,
                    child: Image.asset(
                      'assets/remove/upper.png',
                      fit: BoxFit.fill,
                      width: 200,
                      height: 80,
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
                            'assets/remove/arrow.png',
                            width: 80,
                            height: 80,
                          ),
                        ),
                      ),
                      Text(
                        'Your Posts',
                        style: TextStyle(
                          fontSize: 25.0,
                          fontWeight: FontWeight.bold,
                          color: Color(0xff063221),
                        ),
                      ),
                      SizedBox(
                        width: 80,
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        ),
        body: FutureBuilder<List<RemovedPost>>(
          future: _allPostsFuture,
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return Center(child: CircularProgressIndicator());
            } else if (snapshot.hasError) {
              return Center(child: Text('Error: ${snapshot.error}'));
            } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
              return Center(child: Text('No posts available'));
            } else {
              final posts = snapshot.data!;
              return ListView.builder(
                itemCount: posts.length,
                itemBuilder: (context, index) {
                  return RemoveCard(
                      removedPost: posts[index],
                      onDelete: _deletePost,
                      update: updatepage);
                },
              );
            }
          },
        ),
      ),
    );
  }
}
