import axios from "axios";
import qs from "qs";
const SearchService = {
  async getAll(searchDTO, pageIndex) {
    console.log(searchDTO);
    // Tạo một đối tượng chứa các tham số không null
    const filteredSearchDTO = Object.fromEntries(
        Object.entries(searchDTO).filter(([_, value]) => 
            value !== null && value !== '' && value !== 0
        )
    );

    // Tạo query string, bỏ qua các giá trị null hoặc rỗng
    const queryString = qs.stringify({ 
        pageIndex, 
        ...filteredSearchDTO 
    }, { skipNulls: true });

    try {
        const response = await axios.get(
            `http://localhost:5027/api/User/Post/GetAll?${queryString}`
        );

        if (response.status === 200) {
            console.log(response);
        }

        return response.data;
    } catch (error) {
        console.error('Error fetching data:', error);
        throw error;
    }
}
};
export default SearchService;
