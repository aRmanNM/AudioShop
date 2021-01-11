class CourseEpisode {
  int id;
  String name;
  int price;
  String fileUrl;
  String description;
  int sort;
  String course;

  CourseEpisode({this.id, this.name, this.price, this.fileUrl, this.description, this.sort, this.course});

  CourseEpisode.fromJson(Map<String, dynamic> json)
      : id = json['id'],
        name = json['name'],
        price = json['price'],
        fileUrl = json['fileUrl'],
        description = json['description'],
        sort = json['sort'],
        course = json['course'];

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'price': price,
        'fileUrl': fileUrl,
        'description': description,
        'sort': sort,
        'course': course
      };
}
