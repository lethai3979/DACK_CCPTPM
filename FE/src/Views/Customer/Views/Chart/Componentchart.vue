<template>
    <div>
        <canvas style="max-width: 800px;max-height: 450px;min-width: 800px;min-height: 450px;" ref="chartCanvas"></canvas>
    </div>
</template>

<script>
import { ref, watch, onMounted } from "vue";
import { Chart } from "chart.js";
import ChartDataLabels from "chartjs-plugin-datalabels"; // Import plugin
Chart.register(ChartDataLabels);

export default {
    name: "BarChart",
    props: {
        thongke: {
            type: Array,
            required: true,
        },
    },
    setup(props) {
        const chartCanvas = ref(null);
        let chartInstance = null;

        const createChart = () => {
            if (chartInstance) {
                chartInstance.destroy(); // Hủy biểu đồ cũ trước khi tạo biểu đồ mới
            }

            chartInstance = new Chart(chartCanvas.value, {
                type: "bar",
                data: {
                    labels: props.thongke.map((item) => `${item.month}`),
                    datasets: [
                        {
                            label: "Giá trị",
                            data: props.thongke.map((item) => item.revenue),
                            backgroundColor: props.thongke.map(() => {
                                return `#1d8ca5`; // Màu ngẫu nhiên
                            }),
                            borderColor: "#333",
                            borderWidth: 1,
                        },
                    ],
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            display: true,
                            position: "top",
                        },
                        datalabels: {
                            anchor: "end",
                            align: "top",
                            color: "#000",
                            formatter: (value) => value / 1000 + "k", // Định dạng giá trị
                            font: {
                                weight: "bold",
                            },
                        },
                    },
                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: "Tháng",
                            },
                            ticks: {
                                autoSkip: false, // Hiển thị đầy đủ các tháng
                            },
                        },
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: "Giá trị",
                            },
                            ticks: {
                                stepSize: 1000000,
                                callback: function (revenue) {
                                    return revenue / 1000 + "k"; // Chuyển giá trị thành "100k", "200k", ...
                                },
                            },
                        },
                    },
                },
            });
        };

        // Theo dõi thay đổi dữ liệu và cập nhật biểu đồ
        watch(
            () => props.thongke,
            () => {
                createChart();
            },
            { deep: true }
        );

        onMounted(() => {
            createChart();
        });

        return {
            chartCanvas,
        };
    },
};
</script>

<style scoped>
canvas {
    max-width: 100%;
    margin: auto;
}
</style>
