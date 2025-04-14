let connection = null;
let token = null;

// Login function
async function login() {
    const email = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    try {
        const response = await fetch("http://localhost:5027/api/Authentication/Login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        if (response.ok) {
            const data = await response.json();
            console.log(data.message)
            token = "Bearer " + data.message;
            document.getElementById("userNameDisplay").innerText = email;

            document.getElementById("loginForm").style.display = "none";
            document.getElementById("chatUI").style.display = "block";

            startSignalRConnection(); // Connect to SignalR
        } else {
            alert("Login failed.");
        }
    } catch (error) {
        console.error("Error during login:", error);
    }
}

// Logout function
function logout() {
    token = null;
    if (connection) connection.stop();

    document.getElementById("loginForm").style.display = "block";
    document.getElementById("chatUI").style.display = "none";
}

// Start SignalR connection
function startSignalRConnection() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5027/tracking-hub", {
            accessTokenFactory: () => token.replace("Bearer ", "")
        })
        .build();

    // Listen for ReceiveMessage events
    connection.on("ReceiveMessage", (message) => {
        displayMessage(message);
    });

    connection.start()
        .then(() => console.log("Connected to SignalR Hub"))
        .catch(err => console.error("Error connecting to SignalR Hub:", err));
    console.log(connection.state)
}

// Display message
function displayMessage(message) {
    const messagesDiv = document.getElementById("messages");
    const newMessage = document.createElement("div");
    newMessage.className = "message";
    newMessage.innerText = `${message}`;
    messagesDiv.appendChild(newMessage);
}

async function joinGroup(groupName) {
    try {
        await connection.invoke("AddToGroup", groupName);
        alert(`Joined ${groupName} successfully!`);
    } catch (error) {
        console.log("Error joining group:", error);
    }
}

// Send message function
async function sendMessage(groupName, messageInput) {
    const message = document.getElementById(messageInput).value;

    if (connection && connection.state === "Connected") {
        await connection.invoke("SendMessageToGroup", groupName, message);
        document.getElementById(messageInput).value = ""; // Clear input after sending
    } else {
        alert("You are not connected to the chat hub.");
    }
}
const routeList = [
    {
        id: 1,
        name: 'LongThanh-TamPhuoc',
        strss: [],
        data: [
            { lat: 10.77837147763157, log: 106.94543087885243 },
            { lat: 10.778743733151346, log: 106.94529055965205 },
            { lat: 10.779228, log: 106.946578 },
            { lat: 10.779460, log: 106.947855 },
            { lat: 10.779607477330664, log: 106.9490032037639 },
            { lat: 10.78075628211294, log: 106.94863842336515 },
            { lat: 10.781999937391836, log: 106.94808052393178 },
            { lat: 10.783747270116733, log: 106.947426064971 },
            { lat: 10.784946881752827, log: 106.94682904385893 },
            { lat: 10.785626659556266, log: 106.9481044980529 },
            { lat: 10.786306435823292, log: 106.94898646105939 },
            { lat: 10.787841173125901, log: 106.94815608195943 },
            { lat: 10.79065597018304, log: 106.94688079872594 },
            { lat: 10.792480934412032, log: 106.94625102922792 },
            { lat: 10.795433510167545, log: 106.94523200828354 },
            { lat: 10.79710081812244, log: 106.94484689467099 },
            { lat: 10.797941474018195, log: 106.94477557736951 },
            { lat: 10.79876811668841, log: 106.94454736200474 },
            { lat: 10.799860962828554, log: 106.94443325432236 },
            { lat: 10.800729632565048, log: 106.94426209279881 },
            { lat: 10.801965219076745, log: 106.94420165940727 },
            { lat: 10.80295997889047, log: 106.94418739592352 },
            { lat: 10.803472387537246, log: 106.94427242450548 },
            { lat: 10.804373632240639, log: 106.94429756173302 },
            { lat: 10.806114384771858, log: 106.94446095355968 },
            { lat: 10.807163769718793, log: 106.94464948259046 },
            { lat: 10.807657596659642, log: 106.94477516861097 },
            { lat: 10.80855882873217, log: 106.94485058022329 },
            { lat: 10.810027954717487, log: 106.94496369765207 },
            { lat: 10.811941511386584, log: 106.94463691397998 },
            { lat: 10.813299511896032, log: 106.94423471871431 },
            { lat: 10.814357527710131, log: 106.94386738735432 },
            { lat: 10.815548118564163, log: 106.94352483167872 },
            { lat: 10.814357527710131, log: 106.94386738735432 },
            { lat: 10.815548118564163, log: 106.94352483167872 },
            { lat: 10.816312599197099, log: 106.94331758483801 },
            { lat: 10.817544466315468, log: 106.94294133953794 },
            { lat: 10.818529956366456, log: 106.9427183793601 },
            { lat: 10.819501604330924, log: 106.94238977334848 },
            { lat: 10.821157762483022, log: 106.94187417789024 },
            { lat: 10.822225363789562, log: 106.94133071239509 },
            { lat: 10.823388771032523, log: 106.94075937693941 },
            { lat: 10.823908881045016, log: 106.94053641676157 },
            { lat: 10.826057747053259, log: 106.93933800580575 },
            { lat: 10.826878965710911, log: 106.9388920852416 },
            { lat: 10.828644578095453, log: 106.93805598457475 },
            { lat: 10.829502677164038, log: 106.93747072884416 },
            { lat: 10.82943714667334, log: 106.93767088493688 },
            { lat: 10.831894530261515, log: 106.93623643293901 },
            { lat: 10.833631069170227, log: 106.9354024492193 },
            { lat: 10.836776472491614, log: 106.93363440373355 },
            { lat: 10.838676804314451, log: 106.93246682652598 },
            { lat: 10.839692493960998, log: 106.93153276475992 },
            { lat: 10.842215321443298, log: 106.92959792188388 },
            { lat: 10.843820745798679, log: 106.92913089100084 },
            { lat: 10.846212484469197, log: 106.92919760969842 },
            { lat: 10.848702493445801, log: 106.92913089100084 },
            { lat: 10.85112695594395, log: 106.92879729751297 },
            { lat: 10.85315824722134, log: 106.92856378207145 },
            { lat: 10.855222287118696, log: 106.92769643900297 },
            { lat: 10.857220788353477, log: 106.92699589267842 },
            { lat: 10.857908793881801, log: 106.92662893984176 },
            { lat: 10.859940039247451, log: 106.92606183066772 },
            { lat: 10.86184022383633, log: 106.92506105020408 },
            { lat: 10.86252821872319, log: 106.92432714453075 },
            { lat: 10.86573884056623, log: 106.9228259738353 },
        ],
    },
    {
        id: 2,
        name: 'VoVanNgan-Hutech',
        strss: [],
        data: [
            { lat: 10.842289680694646, log: 106.76294061957648 },
            { lat: 10.842427522172656, log: 106.7632723485643 },
            { lat: 10.84226461860091, log: 106.76339993663652 },
            { lat: 10.84202652860849, log: 106.76354028364958 },
            { lat: 10.842778391105588, log: 106.76431857089018 },
            { lat: 10.843643030669964, log: 106.76479064681091 },
            { lat: 10.84459538436721, log: 106.76532651671431 },
            { lat: 10.845460018663763, log: 106.76583686901846 },
            { lat: 10.844670570065908, log: 106.767265855607 },
            { lat: 10.84614921812136, log: 106.76806966049452 },
            { lat: 10.84705144049693, log: 106.76836311311214 },
            { lat: 10.847585552927796, log: 106.76838265911199 },
            { lat: 10.848879202247293, log: 106.76847287750306 },
            { lat: 10.849720957376155, log: 106.76850896485948 },
            { lat: 10.849738678510956, log: 106.76896005681488 },
            { lat: 10.84965893339435, log: 106.76989832819541 },
            { lat: 10.849667793963729, log: 106.77063811900223 },
            { lat: 10.849650072815022, log: 106.77137790991294 },
            { lat: 10.84964121223403, log: 106.77288455707898 },
            { lat: 10.849348813291538, log: 106.77431000784424 },
            { lat: 10.850571570650038, log: 106.77592491704449 },
            { lat: 10.854208015726645, log: 106.78169204523128 },
            { lat: 10.854703249246922, log: 106.78253425890932 },
            { lat: 10.854792812666329, log: 106.78270055585581 },
            { lat: 10.855514587483151, log: 106.78392364314644 },
            { lat: 10.85608460711603, log: 106.78487624169362 },
            { lat: 10.85618470694377, log: 106.78505326747538 },
            { lat: 10.856348027643318, log: 106.78530539510392 },
            { lat: 10.85650081144273, log: 106.78577746385524 },
            { lat: 10.856226854229611, log: 106.78597058288985 },
            { lat: 10.856005580912553, log: 106.7856058024911 },
            { lat: 10.855894944192563, log: 106.78531612393917 },
            { lat: 10.855705281148696, log: 106.78540731903887 },
            { lat: 10.855441860054706, log: 106.78553606506193 },
            { lat: 10.85522585458433, log: 106.78527857301577 }
        ],
    },
    // thêm các route khác nếu cần
];

function sendRouteToSignalR(groupName, routeId) {
    const selectedRoute = routeList.find(r => r.id === routeId);

    if (!selectedRoute || !Array.isArray(selectedRoute.data) || selectedRoute.data.length === 0) {
        console.error("Route not found or empty.");
        return;
    }

    selectedRoute.data.forEach((point, index) => {
        setTimeout(() => {
            const message = JSON.stringify(point);
            connection.invoke("SendMessageToGroup", groupName, message)
                .then(() => {
                    console.log(`Sent to ${groupName}:`, message);
                })
                .catch(err => console.error("SignalR send error:", err));
        }, index * 2000);
    });
}

function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

