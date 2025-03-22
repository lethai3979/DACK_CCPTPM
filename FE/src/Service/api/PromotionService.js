import PromotionDTO from "../../DTOs/PromotionDto";
import axios from 'axios';
const PromotionService = {

    async AddPromotion(promotionDTO){
        const formData = new PromotionDTO(promotionDTO).toFormData();
        const token = sessionStorage.getItem("authToken");
        const response = await fetch('http://localhost:5027/api/AdminPromotion/Add', {
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
        const response = await fetch(`http://localhost:5027/api/AdminPromotion/Update/${promotionDTO.id}`, {
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
    async getPromotion(id) {
        const response = await axios.get(
            `http://localhost:5027/api/AdminPromotion/GetById/${id}`
        );
        console.log(response.data);
        return response.data;
      },
    async getAllPromotion() {
        const token = sessionStorage.getItem("authToken");
        const response = await axios.get(
            `http://localhost:5027/api/AdminPromotion/GetAll`,{
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
        console.log(response);
        return response;
      },
    async getAllByUserPromotion() {
        const token = sessionStorage.getItem("authToken");
        const response = await axios.get(
            `http://localhost:5027/api/AdminPromotion/GetAllAdminPromotionByUserId`,{
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
        console.log(response);
        return response.data;
      },

}
export default PromotionService;