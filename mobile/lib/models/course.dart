class Course {
  int id;
  String name;
  int price;
  String pictureUrl;
  String description;

  Course({this.id, this.name, this.price, this.pictureUrl, this.description});

  Course.fromJson(Map<String, dynamic> json)
      : id = json['id'],
        name = json['name'],
        price = json['price'],
        pictureUrl = json['pictureUrl'],
        description = json['description'];

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'price': price,
        'pictureUrl': pictureUrl,
        'description': description
      };
}
