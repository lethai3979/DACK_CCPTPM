import PromotionDTO from "../../DTOs/PromotionDto";
import axios from 'axios';
const PostPromotionService = {

    async AddPromotion(promotionDTO){
        const formData = new PromotionDTO(promotionDTO).toFormData();
        const token = sessionStorage.getItem("authToken");
        const response = await fetch('http://localhost:5027/api/UserPromotion/Add', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
            body: formData
        });
        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(
              `${response.status} - ${response.statusText}: ${errorText}`
            );
          }
      
          return response.json();
    },
    async UpdatePromotion(promotionDTO){
        const formData = new PromotionDTO(promotionDTO).toFormData();
        const token = sessionStorage.getItem("authToken");
        const response = await fetch(`http://localhost:5027/api/UserPromotion/Update/${promotionDTO.id}`, {
            method: 'PUT',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
            body: formData
        });
        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(
              `${response.status} - ${response.statusText}: ${errorText}`
            );
          }
      
          return response.json();
    },
    async DeletePromotion(id) {
        const token = sessionStorage.getItem("authToken");
        const response = await axios.post(
            `http://localhost:5027/api/UserPromotion/Delete/${id}`,id,{
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            }
        );
        console.log(response);
        return response;
      },
    async getPromotion(id) {
        const response = await axios.get(
            `http://localhost:5027/api/UserPromotion/GetById/${id}`
        );
        console.log(response.data);
        return response.data;
      },
    async getPromotionbyUser(id) {
        const token = sessionStorage.getItem("authToken");
        const response = await axios.get(
            `http://localhost:5027/api/UserPromotion/GetById/${id}`,{
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            }
        );
        console.log(response.data);
        return response.data;
      },
    async getAllPromotion() {
        const token = sessionStorage.getItem("authToken");
        const response = await axios.get(
            `http://localhost:5027/api/UserPromotion/GetAllByUserId`,{
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
        return response.data;
      },

}
export default PostPromotionService;