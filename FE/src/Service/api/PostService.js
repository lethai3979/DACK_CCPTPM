import PostDTO from "../../DTOs/PostDto";
import PostDto from "../../DTOs/PostDto";
import axios from "axios";
const PostService = {
  async getCarTypies(id) {
    console.log("ID company: ", id);
    const response = await axios.get(
      `http://localhost:5027/api/admin/Company/GetById/${id}`
    );
    console.log(response.data);
    return response.data;
  },
  async getPost(id) {
    const response = await axios.get(
      `http://localhost:5027/api/User/Post/GetById/${id}`
    );
    console.log("Get post: ", response.data);
    return response.data;
  },
  async getAllPostByFindUserId(userId) {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/User/Post/GetAllByUserId/${userId}`
    );
    console.log("Get post: ", response.data);
    return response.data;
  },
  async getAllPostByUserId() {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/User/Post/GetPersonalPosts`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    console.log("Get post: ", response.data);
    return response.data;
  },
  async getAllPost() {
    const token = sessionStorage.getItem("authToken");
    const response = await axios.get(
      `http://localhost:5027/api/User/Post/GetAll`);
    console.log("Get post: ", response.data);
    return response.data;
  },
  async AddPost(postDto) {
    const formData = new PostDTO(postDto).toFormData();
    const token = sessionStorage.getItem("authToken");
    const response = await fetch("http://localhost:5027/api/User/Post/Add", {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
      },
      body: formData,
    });
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `${response.status} - ${response.statusText}: ${errorText}`
      );
    }
    return response;
  },
  async getCompanyById(id) {
    const response = await axios.get(
      `http://localhost:5027/api/admin/Company/GetByIdAsync/${id}`
    );
    console.log(response);
    if (response.status !== 200) {
      throw new Error(`Error: ${response.status} - ${response.statusText}`);
    }
    return response.data;
  },
  async UpdatePost(postDto) {
    const formData = new PostDto(postDto).toFormData();
    console.log(postDto);
    const token = sessionStorage.getItem("authToken");
    const response = await fetch(
      `http://localhost:5027/api/User/Post/Update/${postDto.id}`,
      {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: formData,
      }
    );
    if (!response.ok) {
      const errorText = await response.text();
      throw new Error(
        `${response.status} - ${response.statusText}: ${errorText}`
      );
    }

    return response.json();
  },
};
export default PostService;
