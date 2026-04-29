import { randomString, randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

const categories = ["Italia", "Mexico", "Japanese", "American", "Indian"];

export function generateRandomRestaurant() {
    const randomId = randomString(8);
    return {
        name: `Stress Resto ${randomId}`, // Độ dài > 3 và < 100
        description: `A high-pressure tested restaurant with ID ${randomId}`,
        category: categories[Math.floor(Math.random() * categories.length)],
        hasDelivery: Math.random() < 0.5,
        contactEmail: `contact-${randomId}@stress-test.com`, // Valid email
        contactNumber: `+84${randomIntBetween(100000000, 999999999)}`,
        city: "Test City",
        street: "Performance St",
        postalCode: `${randomIntBetween(10, 99)}-${randomIntBetween(100, 999)}`, // Format XX-XXX
    };
}
