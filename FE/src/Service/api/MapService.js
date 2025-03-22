import axios from "axios";

const MapService = {
  async getMap(lat, lng) {
    try {
      const response = await axios.get(
        `https://nominatim.openstreetmap.org/reverse?lat=${lat}&lon=${lng}&format=json`,
        {
          withCredentials: false
        }
      );
      if (status !== 200) {
        console.log("Đã sinh ra lỗi");
      }
      return response.data;
    } catch (error) {
      console.error();
    }
  },
};

export default MapService;
