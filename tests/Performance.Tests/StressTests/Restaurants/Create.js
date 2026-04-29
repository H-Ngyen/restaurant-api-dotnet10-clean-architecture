import http from 'k6/http';
import { check, sleep } from 'k6';
import { BASE_URL } from '../../common/config.js';
import { setupOwnerAuth } from '../../common/auth.js';
import { generateRandomRestaurant } from '../../common/helpers.js';

// --- CONFIGURATION ---
// Chỉ giữ lại options cấu hình cách mô phỏng người dùng cho riêng file test này
export const options = {
    stages: [
        { duration: '1m', target: 200 },  
        { duration: '5m', target: 200 },  
        { duration: '1m', target: 800 }, 
        { duration: '5m', target: 800 }, 
        { duration: '1m', target: 1000 },
        { duration: '5m', target: 1000 },
        { duration: '5m', target: 0 },   
    ],
    thresholds: {
        http_req_failed: ['rate<0.01'], // Tỷ lệ lỗi phải dưới 1%
        http_req_duration: ['p(95)<500'], // 95% request phải hoàn thành dưới 500ms
    },
};

// --- SETUP PHASE ---
export function setup() {
    const auth = setupOwnerAuth();

    const params = {
        headers: {
            Authorization: `Bearer ${auth.accessToken}`,
            'Content-Type': 'application/json'
        }
    };

    // warmup 10 requests
    for (let i = 0; i < 10; i++) {
        http.post(
          `${BASE_URL}/restaurants`,
          JSON.stringify(generateRandomRestaurant()),
          params
        );
    }

    return auth;
}

// --- MAIN TEST FUNCTION ---
export default function (data) {
    if (!data || !data.accessToken) {
        console.error('No access token provided by setup. Stopping iteration.');
        return;
    }

    const url = `${BASE_URL}/restaurants`;
    const payload = JSON.stringify(generateRandomRestaurant());
    
    const params = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${data.accessToken}`,
        },
    };

    const res = http.post(url, payload, params);

    // Validation
    check(res, {
        'status is 201': (r) => r.status === 201,
        'has id in response': (r) => r.status === 201 && r.headers['Location'] !== undefined,
    });
}
