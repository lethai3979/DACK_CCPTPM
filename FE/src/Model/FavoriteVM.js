import PostVM from "./PostVM";
export default class Favorite {
  constructor(data = {}) {
    this.id = data.id || null;
    this.postId = (data.postId || []).map((post) => new PostVM(post));
  }
}
